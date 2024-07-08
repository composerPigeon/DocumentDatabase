namespace DatabaseNS.Components;

using System.Text;
using System.Text.Json;
using DatabaseNS;

internal class Collection : DatabaseComponent{
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
        throw new InvalidOperationException(ErrorMessages.DOCUMENT_MISSING);
    }

    public Result AddDocument(ComponentName documentName, string content) {
        if (_documents.ContainsKey(documentName))
            throw new InvalidOperationException(ErrorMessages.DOCUMENT_EXIST);

        Document document = Document.Factory.Create(documentName, Path, content);
        _documents.Add(documentName, document);
        _index.AddDocument(document.Stats);
        return new Result("Document was added.");
    }

    public Result RemoveDocument(ComponentName documentName) {
        if (_documents.ContainsKey(documentName)) {
            Document document = _documents[documentName];
            _documents.Remove(documentName);
            _index.RemoveDocument(document.Stats);
            document.Remove();
            return new Result("Document was deleted.");
        }
        throw new InvalidOperationException(ErrorMessages.DOCUMENT_MISSING);
    }

    public void Remove() {
        Path.Remove();
    }

    public void Save() {
        var options = new JsonSerializerOptions {
            WriteIndented = true,
        };
        options.Converters.Add(new NameToStringAsPropertyConverter());
        _index.Save(options);
        foreach (var document in _documents.Values) {
            document.Save(options);
        }
    }

    public Result Find(string[] keyWords) {
        StringBuilder buffer = new StringBuilder();
        foreach (var record in _index.Find(keyWords)) {
            buffer.Append(record.Score.ToString("F"));
            buffer.Append("\t");
            buffer.Append(record.DocumentName);
            buffer.Append('\n');
        }

        return new Result(buffer.ToString());
    }

    public static class Factory {

        public static Collection Create(ComponentName name, ComponentPath path) {
            var documents = new Dictionary<ComponentName, Document>();
            var index = Index.Factory.Create(name, path);
            Directory.CreateDirectory(path.ToString());
            return new Collection(name, path, documents, index);
        } 

        public static Collection Create(ComponentName name, ComponentPath path, Dictionary<ComponentName, Document> documents, Index index) {
            return new Collection(name, path, documents, index);
        }
    }
}