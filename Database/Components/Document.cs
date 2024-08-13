namespace DatabaseNS.Components;

using System.Text.Json;
using DatabaseNS.Components.Builders;
using DatabaseNS.Components.Values;
using DatabaseNS.FileSystem;
using DatabaseNS.ResultNS;
using DatabaseNS.ResultNS.Handlers;

internal class Document : DatabaseComponent {
    public DocumentStats Stats { get; }
    private Document(ComponentName Name, ComponentPath Path, DocumentStats stats) : base(Name, Path) {
        Stats = stats;
    }

    public Result GetContent() {
        string content = FileSystemAccessHandler.ReadDocument(this);
        return Handlers.Result.HandleDocumentReturned(Name, content);
    }

    public static DocumentBuilder CreateBuilder() {
        return new DocumentBuilder((name, path, stats) => new Document(name, path, stats));
    }
}