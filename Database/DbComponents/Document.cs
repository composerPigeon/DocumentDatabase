using System.Text.Json;
using System.Text.Json.Serialization;
using Database_DocumentParser;

namespace Database_DbComponents;

internal class Document : DatabaseComponent {

    [JsonInclude]
    private ComponentPath _contentPath;

    public DocumentStats Stats { get; }

    [JsonConstructor]
    private Document(ComponentName Name, ComponentPath Path, ComponentPath _contentPath, DocumentStats Stats) : base(Name, Path) {
        this.Stats = Stats;
        this._contentPath = _contentPath;
    }

    public Result GetContent() {
        using (var reader = _contentPath.GetReader()) {
            return new Result(reader.ReadToEnd());
        }
    }

    public void Save(JsonSerializerOptions options) {
        string document = JsonSerializer.Serialize(this, options);
        Path.Write(document);
    }

    public void Remove() {
        _contentPath.Remove();
        Path.Remove();
    }
    public static class Factory {
        public static Document Create(ComponentName name, ComponentPath collectionPath, string content) {
            ComponentPath path = collectionPath + name.WithExtension(".json");
            ComponentPath contentPath = collectionPath + name.WithExtension(".txt");
            contentPath.Write(content);
            DocumentStats stats = DocumentParser.Parse(contentPath, name);
            return new Document(name, path, contentPath, stats);
        }
    }   
}