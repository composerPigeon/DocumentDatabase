namespace DatabaseNS.Components.Builders;

using DatabaseNS.Components.Values;
using DatabaseNS.ResultNS.Handlers;

internal class DatabaseBuilder : DatabaseComponentBuilder<Database> {
    public Dictionary<ComponentName, Collection>? Collections { get; set; }

    private Func<Dictionary<ComponentName, Collection>, ComponentPath, Database> _init;

    public DatabaseBuilder(Func<Dictionary<ComponentName, Collection>, ComponentPath, Database> init) {
        _init = init;
    }

    public override Database Build() {
        if (Path.HasValue) {
            if (Collections != null)
                return _init(Collections, Path.Value);
            else
                return _init(new Dictionary<ComponentName, Collection>(), Path.Value);
        } else
            throw Handlers.Exception.ThrowDatabaseCreate();  
    }
}