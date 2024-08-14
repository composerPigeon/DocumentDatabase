namespace DatabaseNS.Components;

using DatabaseNS.Components.Builders;
using DatabaseNS.Components.Values;
using DatabaseNS.FileSystem;
using DatabaseNS.ResultNS;
using DatabaseNS.ResultNS.Handlers;

internal class Database : DatabaseComponent {
    private Dictionary<ComponentName, Collection> _collections;

    private Database(Dictionary<ComponentName, Collection> collections, ComponentPath path) : base(new ComponentName("Database"), path) {
        _collections = collections;
    }

    public Result ListCollections() {
        return Handlers.Result.HandleListCollections(_collections.Keys);
    }

    public Result CreateCollection(ComponentName collectionName) {
        if (!_collections.ContainsKey(collectionName)) {
            var collectionBuilder = Collection.CreateBuilder();
            collectionBuilder.Name = collectionName;
            collectionBuilder.Path = Path.AppendName(collectionName);
            var collection = collectionBuilder.Build();
            FileSystemAccessHandler.AddCollection(collection);
            _collections.Add(collectionName, collection);
            return Handlers.Result.HandleCollectionCreated(collectionName);
        }
        return Handlers.Error.HandleCollectionExists(collectionName);
    }

    public Result RemoveCollection(ComponentName collectionName) {
        if (_collections.ContainsKey(collectionName)) {
            Collection collection = _collections[collectionName];
            FileSystemAccessHandler.RemoveCollection(collection);
            _collections.Remove(collectionName);
            return Handlers.Result.HandleCollectionDropped(collectionName);
        }
        return Handlers.Error.HandleCollectionMissing(collectionName);
    }

    public Result SetTreshhold(ComponentName collectionName, double treshhold) {
        if (_collections.ContainsKey(collectionName))
            return _collections[collectionName].SetTreshhold(treshhold);
        else
            return Handlers.Error.HandleCollectionMissing(collectionName);
    }

    public Result Find(ComponentName collectionName, string[] keyWords) {
        if (_collections.ContainsKey(collectionName)) {
            return _collections[collectionName].Find(keyWords);
        }
        return Handlers.Error.HandleCollectionMissing(collectionName);
    }

    public Result ListDocuments(ComponentName collectionName) {
        if (_collections.ContainsKey(collectionName)) {
            return _collections[collectionName].ListDocuments();
        }
        return Handlers.Error.HandleCollectionMissing(collectionName);
    }

    public Result GetDocument(ComponentName collectionName, ComponentName documentName) {
        if (_collections.ContainsKey(collectionName)) {
            return _collections[collectionName].GetDocument(documentName);
        }
        return Handlers.Error.HandleCollectionMissing(collectionName);
    }

    public Result RemoveDocument(ComponentName collectionName, ComponentName documentName) {
        if(_collections.ContainsKey(collectionName)) {
            return _collections[collectionName].RemoveDocument(documentName);
        }
        return Handlers.Error.HandleCollectionMissing(collectionName);
    }

    public Result AddDocument(ComponentName collectionName, ComponentName documentName, string content) {
        if (_collections.ContainsKey(collectionName)) {
            return _collections[collectionName].AddDocument(documentName, content);
        }
        return Handlers.Error.HandleCollectionMissing(collectionName);
    }

    public Result LoadDocuments(ComponentName collectionName, ComponentPath path) {
        if (!_collections.ContainsKey(collectionName)) {
            var result = CreateCollection(collectionName);
            if (result.Type != ResultType.Ok)
                return result;
        }
        var tuples = new List<Tuple<ComponentName, ComponentPath>>();
        foreach (var entry in FileSystemAccessHandler.ListDirectory(path)) {
            if (entry.EndsWith(".txt"))
                tuples.Add(new Tuple<ComponentName, ComponentPath>(entry.GetComponentName(), entry));
        }
        return _collections[collectionName].LoadDocuments(tuples);
    }

    public static DatabaseBuilder CreateBuilder() {
        return new DatabaseBuilder((collections, path) => new Database(collections, path));
    }
}