namespace DatabaseNS.Components;

using DatabaseNS;
using DatabaseNS.Components.Builders;

internal class Database : DatabaseComponent, IComponentBuildable<Database, DatabaseBuilder> {
    private Dictionary<ComponentName, Collection> _collections;

    private Database(Dictionary<ComponentName, Collection> collections, ComponentPath path) : base(new ComponentName("Database"), path) {
        _collections = collections;
    }

    public void ShutDown() {
        foreach (var collection in _collections.Values) {
            collection.Save();
        }
    }

    public Result CreateCollection(ComponentName collectionName) {
        if (!_collections.ContainsKey(collectionName)) {
            _collections.Add(collectionName, Collection.Create(collectionName, Path + collectionName));
            return new Result("Collection was created.");
        }
        throw new InvalidOperationException(ErrorMessages.COLLECTION_EXIST);
    }

    public Result DropCollection(ComponentName collectionName) {
        if (_collections.ContainsKey(collectionName)) {
            Collection collection = _collections[collectionName];
            _collections.Remove(collectionName);
            collection.Remove();
            return new Result("Collection was deleted.");
        }
        throw new InvalidOperationException(ErrorMessages.COLLECTION_MISSING);
    }

    public Result SetTreshhold(ComponentName collectionName, double treshhold) {
        if (_collections.ContainsKey(collectionName))
            return _collections[collectionName].SetTreshhold(treshhold);
        else
            throw new InvalidOperationException(ErrorMessages.COLLECTION_MISSING);
    }

    public Result GetDocument(ComponentName collectionName, ComponentName documentName) {
        if (_collections.ContainsKey(collectionName)) {
            return _collections[collectionName].GetDocument(documentName);
        }
        throw new InvalidOperationException(ErrorMessages.COLLECTION_MISSING);
    }

    public Result RemoveDocument(ComponentName collectionName, ComponentName documentName) {
        if(_collections.ContainsKey(collectionName)) {
            return _collections[collectionName].RemoveDocument(documentName);
        }
        throw new InvalidOperationException(ErrorMessages.COLLECTION_MISSING);
    }

    public Result AddDocument(ComponentName collectionName, ComponentName documentName, string content) {
        if (_collections.ContainsKey(collectionName)) {
            return _collections[collectionName].AddDocument(documentName, content);
        }
        throw new InvalidOperationException(ErrorMessages.COLLECTION_MISSING);
    }

    public Result Find(ComponentName collectionName, string[] keyWords) {
        if (_collections.ContainsKey(collectionName)) {
            return _collections[collectionName].Find(keyWords);
        }
        throw new InvalidOperationException(ErrorMessages.COLLECTION_MISSING);
    }
    
    public static Database BuildFrom(DatabaseBuilder builder) {
        if (builder.Path.HasValue) {
            var stringPath = builder.Path.Value.ToString();
            if (!Directory.Exists(stringPath))
                Directory.CreateDirectory(stringPath);

            if (builder.Collections != null)
                return new Database(builder.Collections, builder.Path.Value);
            else
                return new Database(new Dictionary<ComponentName, Collection>(), builder.Path.Value);
        } else
            throw new DatabaseCreateException(ErrorMessages.DATABASE_CREATE);      
    }
}