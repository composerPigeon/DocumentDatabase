namespace DatabaseNS.Components.Builders;

using DatabaseNS.Components.IndexNS;
using DatabaseNS.FileSystem;
using DatabaseNS.ResultNS.Handlers;

internal class IndexBuilder : DatabaseComponentBuilder<Index> {
    private Func<ComponentName, ComponentPath, Index> _init;
    public IndexBuilder(Func<ComponentName, ComponentPath, Index> init) {
        _init = init;
    }
    public override Index Build() {
        if (Name.HasValue && Name.Value.IsSafe()) {
            if (Path.HasValue) {
                return _init(Name.Value, Path.Value);
            }
            throw Handlers.Error.ThrowIndexCreate(Name.Value);
        }
        throw Handlers.Error.ThrowComponentNameInvalid(ComponentType.Index, Name);
    }
}