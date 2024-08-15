namespace DatabaseNS.Components.Builders;

using DatabaseNS.Components.IndexNS;
using DatabaseNS.Components.Values;
using DatabaseNS.ResultNS.Handlers;

internal class IndexBuilder : DatabaseComponentBuilder<Index> {
    private Func<ComponentName, ComponentPath, Index> _init;
    public IndexBuilder(Func<ComponentName, ComponentPath, Index> init) {
        _init = init;
    }
    public override Index Build() {
        if (Name.HasValue) {
            if (Path.HasValue) {
                return _init(Name.Value, Path.Value);
            }
            throw Handlers.Exception.ThrowIndexCreate(Name.Value);
        }
        throw Handlers.Exception.ThrowComponentNameInvalid(ComponentType.Index, Name);
    }
}