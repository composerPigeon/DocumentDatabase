namespace DatabaseNS.Components;

using System.Text;
using System.Text.Json;
using DatabaseNS;
using DatabaseNS.Components.Builders;
using DatabaseNS.Components.IndexNS;

internal class Collection : DatabaseComponent, IComponentCreatable<Collection>, IComponentBuildable<Collection, CollectionBuilder> {
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

        Document document = DatabaseLoader.AddNewDocument(documentName, Path, content);
        document.Path.Write(content);
        _documents.Add(documentName, document);
        _index.AddDocument(documentName, document.Stats);
        return new Result("Document was added.");
    }

    public Result RemoveDocument(ComponentName documentName) {
        if (_documents.ContainsKey(documentName)) {
            Document document = _documents[documentName];
            _documents.Remove(documentName);
            _index.RemoveDocument(documentName, document.Stats);
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

    public static Collection Create(ComponentName name, ComponentPath path) {
        ComponentPath indexPath = path + DatabaseLoader.INDEX_DIRECTORY + "index.json".AsPath();
        if (!Directory.Exists(path.ToString()))
            Directory.CreateDirectory(path.ToString());
        if (!Directory.Exists(indexPath.ToString()))
            Directory.CreateDirectory(indexPath.ToString());
        return new Collection(
            name,
            path,
            new Dictionary<ComponentName, Document>(),
            Index.Create(name.Concat("_index"), indexPath)
        );
    }

    public static Collection BuildFrom(CollectionBuilder builder) {
        if (builder.Name.HasValue && builder.Path.HasValue) {
            if (builder.Index != null && builder.Documents != null) {
                return new Collection(
                    builder.Name.Value,
                    builder.Path.Value,
                    builder.Documents,
                    builder.Index
                );
            } else {
                return Create(builder.Name.Value, builder.Path.Value);
            }
        } else 
            throw new DatabaseCreateException(ErrorMessages.COLLECTION_CREATE);
    }
}