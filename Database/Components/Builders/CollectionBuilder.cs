namespace DatabaseNS.Components.Builders;

using DatabaseNS.Components.IndexNS;
using DatabaseNS.FileSystem;
using DatabaseNS.ResultNS.Handlers;

internal class CollectionBuilder : DatabaseComponentBuilder<Collection> {
    public Index? Index { get; set; }
    public Dictionary<ComponentName, Document>? Documents { get; set; }

    private Func<ComponentName, ComponentPath, Dictionary<ComponentName, Document>, Index, Collection> _init;

    public CollectionBuilder(Func<ComponentName, ComponentPath, Dictionary<ComponentName, Document>, Index, Collection> init) {
        _init = init;
    }

    private Index buildIndex(ComponentName collectionName, ComponentPath collectionPath) {
        var index = Index.CreateBuilder();
        var indexPath = EntryCreator.CreateCollectionDirectories(collectionPath);
        index.Name = collectionName.AppendString("_index");
        index.Path = indexPath;
        return index.Build();
    }

    public override Collection Build() {
        if (Name.HasValue && Path.HasValue) {
            if (Index != null && Documents != null) {
                return _init(
                    Name.Value,
                    Path.Value,
                    Documents,
                    Index
                );
            } else {
                return _init(
                    Name.Value,
                    Path.Value,
                    new Dictionary<ComponentName, Document>(),
                    buildIndex(Name.Value, Path.Value)
                );
            }
        } else 
            throw Handlers.Error.ThrowComponentNameInvalid(ComponentType.Collection, Name);
    }
}