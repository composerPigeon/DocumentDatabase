namespace DatabaseNS.Components;

using System.Text;
using System.Text.Json;
using DatabaseNS.Components.Builders;
using DatabaseNS.FileSystem;
using DatabaseNS.Components.IndexNS;
using DatabaseNS.ResultNS.Handlers;
using DatabaseNS.ResultNS;

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

    public Result GetDocument(ComponentName documentName) {
        if (_documents.ContainsKey(documentName)) {
            return _documents[documentName].GetContent();
        }
        return Handlers.Error.HandleDocumentMissing(documentName);
    }

    private Document createDocument(ComponentName documentName, string content) {
        DocumentStats stats = DocumentStats.ReadDocument(
            documentName.AppendString("_stats"),
            EntryCreator.GetIndexDirectoryFor(Path)
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
        EntryCreator.CreateDocumentFile(document.Path, content);
        _documents.Add(documentName, document);
        _index.AddDocument(documentName, document.Stats);
        return Handlers.Result.HandleDocumentAdded(documentName);
    }

    public Result RemoveDocument(ComponentName documentName) {
        if (_documents.ContainsKey(documentName)) {
            Document document = _documents[documentName];
            _documents.Remove(documentName);
            _index.RemoveDocument(documentName, document.Stats);
            document.Remove();
            return Handlers.Result.HandleDocumentRemoved(documentName);
        }
        return Handlers.Error.HandleDocumentMissing(documentName);
    }

    public void Remove() {
        Path.AsExecutable().Remove();
    }

    public void Save() {
        var options = new JsonSerializerOptions {
            WriteIndented = true,
        };
        options.Converters.Add(new NameToStringAsPropertyConverter());
        _index.Save(options);
    }

    public Result Find(string[] keyWords) {
        StringBuilder buffer = new StringBuilder();
        foreach (var record in _index.Find(keyWords)) {
            buffer.Append(record.Score.ToString("F"));
            buffer.Append("\t");
            buffer.Append(record.DocumentName);
            buffer.Append('\n');
        }

        return Handlers.Result.HandleQueryResult(Name, buffer.ToString());
    }

    public static CollectionBuilder CreateBuilder() {
        return new CollectionBuilder((name, path, documents, index) => new Collection(name, path, documents, index));
    }
}