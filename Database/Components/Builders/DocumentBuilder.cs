namespace DatabaseNS.Components.Builders;


using DatabaseNS.FileSystem;
using DatabaseNS.ResultNS.Handlers;

internal class DocumentBuilder : DatabaseComponentBuilder<Document> {

    public DocumentStats? Stats { get; set; }

    private Func<ComponentName, ComponentPath, DocumentStats, Document> _init;

    public DocumentBuilder(Func<ComponentName, ComponentPath, DocumentStats, Document> init) {
        _init = init;
    }

    public override Document Build() {
        if (Name.HasValue && Name.Value.IsSafe()) {
            if (Stats != null && Path.HasValue) {
                return _init(
                    Name.Value,
                    Path.Value,
                    Stats
                );
            } else {
                throw Handlers.Error.ThrowDocumentCreate(Name.Value);
            }
        } else {
            throw Handlers.Error.ThrowComponentNameInvalid(ComponentType.Document, Name);
        }
    }
}