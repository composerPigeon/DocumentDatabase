namespace DatabaseNS.Components.Builders;

using DatabaseNS.Components.Values;
using DatabaseNS.ResultNS.Handlers;

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
            throw Handlers.Exception.ThrowDocumentStatsCreate(Name.Value);
        }
        throw Handlers.Exception.ThrowComponentNameInvalid(ComponentType.DocumentStats, Name);
    }
}