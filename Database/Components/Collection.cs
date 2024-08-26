namespace DatabaseNS.Components;

using DatabaseNS.Components.Builders;
using DatabaseNS.Components.Values;
using DatabaseNS.Components.IndexNS;
using DatabaseNS.ResultNS.Handlers;
using DatabaseNS.ResultNS;
using DatabaseNS.FileSystem;
using DatabaseNS.ResultNS.Exceptions;

internal class Collection : DatabaseComponent, IDatabaseComponentBuilderCreatable<Collection, CollectionBuilder>{
    private Dictionary<ComponentName, Document> _documents;
    private Index _index;

    private Collection(ComponentName name, ComponentPath path, Dictionary<ComponentName, Document> documents, Index index) : base(name, path) {
        _documents = documents;
        _index = index;
    }

    public Result SetTreshhold(double treshhold) {
        return _index.SetTreshhold(treshhold);
    }

    public Result Find(string[] keyWords) {
        return Handlers.Result.HandleQueryResult(Name, _index.Find(keyWords));
    }

    public Result ListDocuments() {
        return Handlers.Result.HandleListDocuments(Name, _documents.Keys);
    }

    public Result GetDocument(ComponentName documentName) {
        if (_documents.ContainsKey(documentName)) {
            return _documents[documentName].GetContent();
        }
        return Handlers.Error.HandleDocumentMissing(documentName);
    }

    private DocumentStats createDocumentStats(ComponentName documentName, string content) {
        return DocumentStats.Create(
            documentName.AppendString("_stats"),
            FileSystemAccessHandler.GetIndexDirectoryPath(Path)
                .AppendPath(documentName.WithExtension(".json")),
            content
        );
    }

    private DocumentStats createDocumentStats(ComponentName documentName, ComponentPath filePath) {
        return DocumentStats.Create(
            documentName.AppendString("_stats"),
            FileSystemAccessHandler.GetIndexDirectoryPath(Path)
                .AppendPath(documentName.WithExtension(".json")),
            filePath
        );
    }

    private Document createDocument(ComponentName documentName, DocumentStats stats) {
        DocumentBuilder builder = Document.CreateBuilder();
        builder.Name = documentName;
        builder.Path = Path.AppendPath(documentName.WithExtension(".txt"));
        builder.Stats = stats;
        return builder.Build();
    }

    public Result AddDocument(ComponentName documentName, string content) {
        if (_documents.ContainsKey(documentName))
            return Handlers.Error.HandleDocumentExists(documentName);

        Document document = createDocument(documentName, createDocumentStats(documentName, content));
        FileSystemAccessHandler.AddDocument(document, content);
        try {
            _index.AddDocument(document);
        } catch (ResultException) {
            FileSystemAccessHandler.RemoveDocument(document);
            throw;
        }
        _documents.Add(documentName, document);
        return Handlers.Result.HandleDocumentAdded(documentName);
    }

    public Result LoadDocuments(ICollection<Tuple<ComponentName, ComponentPath>> tuples) {
        var addedDocuments = new List<Document>();
        try {
            if (tuples.Count == 0) {
                return Handlers.Error.HandleLoadDocumentsEmpty(Name);
            }
            if (tuples.All(t => !_documents.ContainsKey(t.Item1))) {
                foreach(var tuple in tuples) {
                    Document document = createDocument(tuple.Item1, createDocumentStats(tuple.Item1, tuple.Item2));
                    addedDocuments.Add(document);
                    _documents.Add(document.Name, document); 
                    FileSystemAccessHandler.AddDocument(document, tuple.Item2);
                }
                _index.AddDocuments(addedDocuments);
                return Handlers.Result.HandleDocumentsLoaded(Name);
            } else {
                return Handlers.Error.HandleLoadDocumentsSomeExisted(Name);
            }
        } catch (ResultException) {
            foreach(var document in addedDocuments) {
                _documents.Remove(document.Name);
                FileSystemAccessHandler.RemoveDocument(document);
            }
            throw;
        }
    }

    public Result RemoveDocument(ComponentName documentName) {
        if (_documents.ContainsKey(documentName)) {
            Document document = _documents[documentName];
            _index.RemoveDocument(document);
            try {
                FileSystemAccessHandler.RemoveDocument(document);
            } catch (ResultException) {
                _index.AddDocument(document);
                throw;
            }
            _documents.Remove(documentName);
            return Handlers.Result.HandleDocumentRemoved(documentName);
        }
        return Handlers.Error.HandleDocumentMissing(documentName);
    }

    public static CollectionBuilder CreateBuilder() {
        return new CollectionBuilder((name, path, documents, index) => new Collection(name, path, documents, index));
    }
}