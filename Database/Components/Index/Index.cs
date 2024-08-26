namespace DatabaseNS.Components.IndexNS;

using System.Text.Json.Serialization;
using DatabaseNS.Components.Builders;
using DatabaseNS.Components.Values;
using DatabaseNS.FileSystem;
using DatabaseNS.ResultNS;
using DatabaseNS.ResultNS.Exceptions;
using DatabaseNS.ResultNS.Handlers;

// Implementation of inverted index in database
internal class Index : DatabaseComponent, IDatabaseComponentBuilderCreatable<Index, IndexBuilder>{

    [JsonInclude]
    private ulong _documentCount;

    //for each word contains count of documents, where the word is present
    [JsonInclude]
    private Dictionary<string, ulong> _wordDocumentCounts;

    // for each word contains list of documents, where the word is present and also word's relative frequency in the document
    [JsonInclude]
    private Dictionary<string, SortedList<ComponentName, double>> _wordByDocumentTF;

    [JsonInclude]
    private double _queryTreshhold;

    private Index(ComponentName name, ComponentPath path ) : base(name, path) {
        _wordByDocumentTF = new Dictionary<string, SortedList<ComponentName, double>>();
        _wordDocumentCounts = new Dictionary<string, ulong>();
        _documentCount = 0;
        _queryTreshhold = 0.5;
    }

    [JsonConstructor]
    private Index(ComponentName Name, ComponentPath Path, Dictionary<string, SortedList<ComponentName, double>> _wordByDocumentTF, Dictionary<string, ulong> _wordDocumentCounts, ulong _documentCount, double _queryTreshhold) : base(Name, Path) {
        this._wordByDocumentTF = _wordByDocumentTF;
        this._wordDocumentCounts = _wordDocumentCounts;
        this._documentCount = _documentCount;
        this._queryTreshhold = _queryTreshhold;
    }

    private double calculateIDF(string term) {
        if (_wordDocumentCounts.ContainsKey(term))
            return Math.Log2((double)_documentCount/_wordDocumentCounts[term]);
        else
            return 0;
    }

    public Result SetTreshhold(double treshhold) {
        if (treshhold <= 1 && treshhold >= 0) {
            _queryTreshhold = treshhold;
            return Handlers.Result.HandleTreshholdSet(treshhold);
        } else
            return Handlers.Error.HandleInvalidTreshholdInterval(treshhold);
        
    }

    // reads the document statistics and incrementally update the term frequencies and document counts in index
    private void addDocument(ComponentName documentName, DocumentStats stats) {
        _documentCount += 1;
        foreach(var entry in stats.WordsTF) {
            if (_wordDocumentCounts.ContainsKey(entry.Key))
                _wordDocumentCounts[entry.Key] += 1;
            else
                _wordDocumentCounts[entry.Key] = 1;
            
            if (!_wordByDocumentTF.ContainsKey(entry.Key))
                _wordByDocumentTF[entry.Key] = new SortedList<ComponentName, double>();
            _wordByDocumentTF[entry.Key].Add(documentName, entry.Value);
        }
    }

    public void AddDocument(Document document) {
        addDocument(document.Name, document.Stats);
        try {
            FileSystemAccessHandler.SaveIndex(this);
        } catch (ResultException) {
            removeDocument(document.Name, document.Stats);
            throw;
        }
    }

    public void AddDocuments(IEnumerable<Document> documents) {
        var addedDocuments = new List<Document>();
        foreach(var document in documents) {
            addDocument(document.Name, document.Stats);
            addedDocuments.Add(document);
        }

        try {
            FileSystemAccessHandler.SaveIndex(this);
        } catch (ResultException) {
            foreach(var document in addedDocuments) {
                removeDocument(document.Name, document.Stats);
            }
            throw;
        }
        
    }

    // substract term frequiencies from index based on document values and thus remove the document from index
    private void removeDocument(ComponentName documentName, DocumentStats stats) {
        _documentCount -= 1;
        foreach(var entry in stats.WordsTF) {
            if (_wordDocumentCounts.ContainsKey(entry.Key) && _wordDocumentCounts[entry.Key] > 1)
                _wordDocumentCounts[entry.Key] -= 1;
            else if (_wordDocumentCounts.ContainsKey(entry.Key)) {
                _wordDocumentCounts.Remove(entry.Key);
            }
            if (_wordByDocumentTF.ContainsKey(entry.Key))
                _wordByDocumentTF[entry.Key].Remove(documentName); //Remove returns false if element is not present
        }
    }

    public void RemoveDocument(Document document) {
        removeDocument(document.Name, document.Stats);
        try {
            FileSystemAccessHandler.SaveIndex(this);
        } catch (ResultException) {
            addDocument(document.Name, document.Stats);
            throw;
        }
        
    }

    // query is term frequencies for inputted key words, document is vector representing term frequencies of these keywords in specific document, idf = inverse document frequencies of this document in collection
    private double calculateCosineSimilarity(double[] query, double[] document, double[] idfs) {
        if (query.Length == document.Length) {
            double result = 0;
            double normQuery = 0;
            double normDocument = 0;
            for (int i = 0; i < query.Length; i++) {
                result += query[i] * document[i] * idfs[i];
                normQuery += query[i] * query[i];
                normDocument += document[i] * idfs[i] * document[i] * idfs[i];
            }
            return result / (Math.Sqrt(normQuery) * Math.Sqrt(normDocument));
        }
        throw Handlers.Exception.ThrowQueryInvalid(query.Length, document.Length);
    }

    // calculate the term frequencies for inputted key words 
    private Dictionary<string, double> getQueryStats(string[] keyWords) {
        var tfresult = new Dictionary<string, int>();
        int maxCount = 0;

        foreach (var word in keyWords) {
            if (tfresult.ContainsKey(word))
                tfresult[word] += 1;
            else
                tfresult[word] = 1;
            if (tfresult[word] > maxCount)
                maxCount = tfresult[word];
        }
        
        var result = new Dictionary<string, double>();
        foreach(var pair in tfresult) {
            result[pair.Key] = (double)pair.Value / maxCount;
        }
        return result;
    }

    private double[] getQueryIDFs(string[] keyWords) {
        List<double> result = new List<double>();
        foreach (var word in keyWords) {
            result.Add(calculateIDF(word));
        }
        return result.ToArray();
    }

    // calculates document vector for specified set of keywords (query)
    private double[] getQueryDocument(Dictionary<string, double> queryStats, double[] idfs, string[] keyWords) {
        List<double> result = new List<double>();
        if (queryStats.Count == idfs.Length && queryStats.Count == keyWords.Length) {
            for (int i = 0; i < keyWords.Length; i++) {
                string word = keyWords[i];
                result.Add(queryStats[word] * idfs[i]);
            }
        }
        return result.ToArray();
    }

    // Iterates through relative documents and calculates the result of ind command
    public List<IndexRecord> Find(string[] inputKeyWords) {
        Dictionary<string, double> queryStats = getQueryStats(inputKeyWords);
        string[] keyWords = queryStats.Keys.ToArray();
        double[] queryIDFs = getQueryIDFs(keyWords);
        double[] query = getQueryDocument(queryStats, queryIDFs, keyWords);
        var result = new List<IndexRecord>();

        IndexQuery indexQuery = new IndexQuery(keyWords, _wordByDocumentTF);

        foreach (IndexQueryRecord record in indexQuery) {
            double recordScore = calculateCosineSimilarity(query, record.Values, queryIDFs);
            if (recordScore >= _queryTreshhold)
                result.Add(new IndexRecord(record.Name, recordScore));
        }
        result.Sort();
        return result;
    }

    public static IndexBuilder CreateBuilder() {
        return new IndexBuilder((name, path) => new Index(name, path));
    }
}
