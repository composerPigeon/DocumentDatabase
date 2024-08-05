namespace DatabaseNS.Components.Builders;

using DatabaseNS.FileSystem;
using DatabaseNS.ResultNS.Handlers;

internal class DatabaseBuilder : DatabaseComponentBuilder<Database> {
    public Dictionary<ComponentName, Collection>? Collections { get; set; }

    public Func<Dictionary<ComponentName, Collection>, ComponentPath, Database> _init;

    public DatabaseBuilder(Func<Dictionary<ComponentName, Collection>, ComponentPath, Database> init) {
        _init = init;
    }

    public override Database Build() {
        if (Path.HasValue) {
            if (!Directory.Exists(Path.Value))
                Directory.CreateDirectory(Path.Value);

            if (Collections != null)
                return _init(Collections, Path.Value);
            else
                return _init(new Dictionary<ComponentName, Collection>(), Path.Value);
        } else
            throw Handlers.Error.ThrowDatabaseCreate();  
    }
}