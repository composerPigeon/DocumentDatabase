namespace DatabaseNS.Components;

using System.Text.Json;
using DatabaseNS.Components.Builders;
using DatabaseNS.FileSystem;
using DatabaseNS.ResultNS;
using DatabaseNS.ResultNS.Handlers;

internal class Document : DatabaseComponent {
    public DocumentStats Stats { get; }
    private Document(ComponentName Name, ComponentPath Path, DocumentStats stats) : base(Name, Path) {
        Stats = stats;
    }

    public Result GetContent() {
        string content = Path.AsExecutable().Read();
        return Handlers.Result.HandleDocumentReturned(Name, content);
    }

    public void Save(JsonSerializerOptions options) {
        Stats.Save(options);
    }

    public void Remove() {
        Path.AsExecutable().Remove();
        Stats.Remove();
    }

    public static DocumentBuilder CreateBuilder() {
        return new DocumentBuilder((name, path, stats) => new Document(name, path, stats));
    }
}