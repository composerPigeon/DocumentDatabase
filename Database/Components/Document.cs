namespace DatabaseNS.Components;

using System.Text.Json;
using System.Text.Json.Serialization;
using DatabaseNS.Components.Builders;
using DatabaseNS.DocumentParserNS;

internal class Document : DatabaseComponent, IComponentBuildable<Document, DocumentBuilder> {
    public DocumentStats Stats { get; }
    private Document(ComponentName Name, ComponentPath Path, DocumentStats stats) : base(Name, Path) {
        Stats = stats;
    }

    public Result GetContent() {
        using (var reader = Path.GetReader()) {
            return new Result(reader.ReadToEnd());
        }
    }

    public void Save(JsonSerializerOptions options) {
        Stats.Save(options);
    }

    public void Remove() {
        Path.Remove();
        Stats.Remove();
    }

    public static Document BuildFrom(DocumentBuilder builder) {
        if (builder.Stats != null && builder.Name.HasValue && builder.Path.HasValue) {
            return new Document(
                builder.Name.Value,
                builder.Path.Value,
                builder.Stats
            );
        } else
            throw new DatabaseCreateException(ErrorMessages.DOCUMENT_CREATE);
    }  
}