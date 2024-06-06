using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using Database_CommandParser;
using DatabaseNS;

namespace Database_DbComponents;

internal struct IndexRecord : IComparable<IndexRecord> {
    public ComponentName DocumentName {get;}
    public double Score {get;}

    public int CompareTo(IndexRecord other) {
        return -Score.CompareTo(other.Score);
    }

    public IndexRecord(ComponentName name, double score) {
        DocumentName = name;
        Score = score;
    }
}

internal class Index : DatabaseComponent {

    [JsonInclude]
    private ulong _documentCount;

    [JsonInclude]
    private Dictionary<string, ulong> _wordDocumentCounts;

    [JsonInclude]
    private Dictionary<string, SortedList<ComponentName, double>> _wordByDocumentTF;

    private Index(ComponentName name, ComponentPath path ) : base(name, path) {
        _wordByDocumentTF = new Dictionary<string, SortedList<ComponentName, double>>();
        _wordDocumentCounts = new Dictionary<string, ulong>();
        _documentCount = 0;
    }

    [JsonConstructor]
    private Index(ComponentName Name, ComponentPath Path, Dictionary<string, SortedList<ComponentName, double>> _wordByDocumentTF, Dictionary<string, ulong> _wordDocumentCounts, ulong _documentCount) : base(Name, Path) {
        this._wordByDocumentTF = _wordByDocumentTF;
        this._wordDocumentCounts = _wordDocumentCounts;
        this._documentCount = _documentCount;
    }

    private double calculateIDF(string term) {
        if (_wordDocumentCounts.ContainsKey(term))
            return Math.Log2((double)_documentCount/_wordDocumentCounts[term]);
        else
            return 0;
    }

    public void AddDocument(DocumentStats stats) {
        _documentCount += 1;
        foreach(var entry in stats.WordsTF) {
            if (_wordByDocumentTF.ContainsKey(entry.Key))
                _wordDocumentCounts[entry.Key] += 1;
            else
                _wordDocumentCounts[entry.Key] = 1;
            
            if (!_wordByDocumentTF.ContainsKey(entry.Key))
                _wordByDocumentTF[entry.Key] = new SortedList<ComponentName, double>();
            _wordByDocumentTF[entry.Key].Add(stats.DocumentName, entry.Value);
        }
    }

    public void LoadDocuments(DocumentStats[] stats) {
        foreach (DocumentStats documentStats in stats) {
            AddDocument(documentStats);
        }
    }

    public void RemoveDocument(DocumentStats stats) {
        _documentCount -= 1;
        foreach(var entry in stats.WordsTF) {
            if (_wordByDocumentTF.ContainsKey(entry.Key) && _wordDocumentCounts[entry.Key] > 1)
                _wordDocumentCounts[entry.Key] -= 1;
            else if (_wordByDocumentTF.ContainsKey(entry.Key)) {
                _wordDocumentCounts.Remove(entry.Key);
            }
            if (_wordByDocumentTF.ContainsKey(entry.Key))
                _wordByDocumentTF[entry.Key].Remove(stats.DocumentName);
        }
    }

    public void RemoveDocuments(DocumentStats[] stats) {
        foreach (DocumentStats documentStats in stats) {
            RemoveDocument(documentStats);
        }
    }

    private double[] calculateTfIdfForDocument(double[] tfDocument, double[] idf) {
        List<double> result = new List<double>();
        if (tfDocument.Length == idf.Length) {
            for (int i = 0; i < tfDocument.Length; i++) {
                result.Add(tfDocument[i] * idf[i]);
            }
        }
        return result.ToArray();
    }

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
        throw new InvalidOperationException(ErrorMessages.QUERY_INVALID);
    }

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

    public List<IndexRecord> Find(string[] inputKeyWords) {
        Dictionary<string, double> queryStats = getQueryStats(inputKeyWords);
        string[] keyWords = queryStats.Keys.ToArray();
        double[] queryIDFs = getQueryIDFs(keyWords);
        double[] query = getQueryDocument(queryStats, queryIDFs, keyWords);
        var result = new List<IndexRecord>();

        IndexQuery indexQuery = new IndexQuery(keyWords, _wordByDocumentTF);

        foreach (IndexQueryRecord record in indexQuery) {
            result.Add(new IndexRecord(record.Name, calculateCosineSimilarity(query, record.Values, queryIDFs)));
        }
        result.Sort();
        return result;
    }

    public void Save(JsonSerializerOptions options) {
        string jsonIndex = JsonSerializer.Serialize(this, options);
        Path.Write(jsonIndex);
    }

    public static class Factory {
        public static Index Create(ComponentName collectionName, ComponentPath collectionPath) {
            ComponentName name = collectionName.Concat("_index");
            ComponentPath path = collectionPath + new ComponentName("index").WithExtension(".json");
            return new Index(name, path);
        }
    }
}
