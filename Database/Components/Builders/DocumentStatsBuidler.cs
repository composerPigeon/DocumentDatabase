using DatabaseNS.FileSystem;
using DatabaseNS.ResultNS.Handlers;

namespace DatabaseNS.Components.Builders;

internal class DocumentStatsBuilder : DatabaseComponentBuilder<DocumentStats> {
    private Func<ComponentName, ComponentPath, Dictionary<string, double>, DocumentStats> _init;
    public DocumentStatsBuilder(Func<ComponentName, ComponentPath, Dictionary<string, double>, DocumentStats> init) {
        _init = init;
    }
    public Dictionary<string, double>? WordsTF {get; set;}

    public override DocumentStats Build() {
        if (Name.HasValue && Name.Value.IsSafe()) {
            if (Path.HasValue && WordsTF != null) {
                return _init(Name.Value, Path.Value, WordsTF);
            }
            throw Handlers.Error.ThrowDocumentStatsCreate(Name.Value);
        }
        throw Handlers.Error.ThrowComponentNameInvalid(ComponentType.DocumentStats, Name);
    }
}