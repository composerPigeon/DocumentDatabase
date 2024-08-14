namespace DatabaseNS.Components;

using System.Text;

using DatabaseNS.Components.Builders;
using DatabaseNS.Components.Values;
using DatabaseNS.Components.IndexNS;
using DatabaseNS.ResultNS.Handlers;
using DatabaseNS.ResultNS;
using DatabaseNS.FileSystem;

internal class Collection : DatabaseComponent {
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

    private Document createDocument(ComponentName documentName, string content) {
        DocumentStats stats = DocumentStats.ReadDocument(
            documentName.AppendString("_stats"),
            FileSystemAccessHandler.GetIndexDirectoryPath(Path)
                .AppendPath(documentName.WithExtension(".json")),
            content
        );

        DocumentBuilder builder = Document.CreateBuilder();
        builder.Name = documentName;
        builder.Path = Path.AppendPath(documentName.WithExtension(".txt"));
        builder.Stats = stats;
        return builder.Build();
    }

    public Result AddDocument(ComponentName documentName, string content) {
        if (_documents.ContainsKey(documentName))
            return Handlers.Error.HandleDocumentExists(documentName);

        Document document = createDocument(documentName, content);
        FileSystemAccessHandler.AddDocument(document, content);
        _documents.Add(documentName, document);
        _index.AddDocument(documentName, document.Stats);
        return Handlers.Result.HandleDocumentAdded(documentName);
    }

    public Result RemoveDocument(ComponentName documentName) {
        if (_documents.ContainsKey(documentName)) {
            Document document = _documents[documentName];
            _documents.Remove(documentName);
            _index.RemoveDocument(documentName, document.Stats);
            FileSystemAccessHandler.RemoveDocument(document);
            return Handlers.Result.HandleDocumentRemoved(documentName);
        }
        return Handlers.Error.HandleDocumentMissing(documentName);
    }

    public static CollectionBuilder CreateBuilder() {
        return new CollectionBuilder((name, path, documents, index) => new Collection(name, path, documents, index));
    }
}