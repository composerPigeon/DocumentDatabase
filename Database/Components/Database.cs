namespace DatabaseNS.Components;

using DatabaseNS.DocumentParserNS;
using DatabaseNS;

using System.Text.Json;

internal class DatabaseLoadException : Exception {
    public DatabaseLoadException(string message) : base(message) { }
}

internal class Database : DatabaseComponent {
    private Dictionary<ComponentName, Collection> _collections;

    private Database(Dictionary<ComponentName, Collection> collections, ComponentPath path) : base(new ComponentName("Database"), path) {
        _collections = collections;
    }

    public void ShutDown() {
        foreach (var collection in _collections.Values) {
            collection.Save();
        }
    }

    public Result CreateColletion(ComponentName collectionName) {
        if (!_collections.ContainsKey(collectionName)) {
            _collections.Add(collectionName, Collection.Factory.Create(collectionName, Path + collectionName));
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

    public static class Factory {
        public static Database Create(ComponentPath path) {
            DocumentParser.StopWords = loadStopWords(path);

            var collections = loadCollections(path);
            return new Database(collections, path);
        }

        private static HashSet<string> loadStopWords(ComponentPath dbPath) {
            try {
                ComponentName stopWordsName = "stopWords".ToName();
                ComponentPath path = dbPath + stopWordsName.WithExtension(".json");
                var options = new JsonSerializerOptions();
                HashSet<string>? set = path.LoadAsJson<HashSet<string>>(options);
                if (set != null)
                    return set;
                else
                    throw new DatabaseLoadException(ErrorMessages.STOPWORDS_LOAD);
            } catch (DatabaseLoadException) {
                throw;
            } catch (JsonException){
                throw new DatabaseLoadException(ErrorMessages.STOPWORDS_LOAD);
            }
        }

        private static Dictionary<ComponentName, Collection> loadCollections(ComponentPath path) {
            var collections = new Dictionary<ComponentName, Collection>();
            foreach (var collectionPath in path.List()) {
                if (!collectionPath.EndsWith("stopWords.json")) {
                    Collection collection = loadCollection(collectionPath);
                    collections.Add(collection.Name, collection);
                }
            }
            return collections;
        }

        private static Collection loadCollection(ComponentPath path) {
            var documents = new Dictionary<ComponentName, Document>();
            Index? index = null;

            var options = new JsonSerializerOptions() {
                WriteIndented = true,
            };
            options.Converters.Add(new NameToStringAsPropertyConverter());

            foreach(var componentPath in path.List()) {
                if (componentPath.EndsWith("index.json")) {
                    index = loadIndex(componentPath, options);
                } else if (componentPath.EndsWith(".json")) {
                    Document document = loadDocument(componentPath, options);
                    documents.Add(document.Name, loadDocument(componentPath, options));
                }
            }

            if (index != null)
                return Collection.Factory.Create(path.GetDirectoryName(), path, documents, index);
            else
                throw new DatabaseLoadException(ErrorMessages.INDEX_LOAD);
        }

        private static Index loadIndex(ComponentPath path, JsonSerializerOptions options) {
            try {
                Index? index = path.LoadAsJson<Index>(options);
                if (index != null)
                    return index;
                else
                    throw new DatabaseLoadException(ErrorMessages.INDEX_LOAD);
            } catch (DatabaseLoadException) {
                throw;
            } catch (JsonException){
                throw new DatabaseLoadException(ErrorMessages.INDEX_LOAD);
            }
        }

        private static Document loadDocument(ComponentPath path, JsonSerializerOptions options) {
            try {
                Document? document = path.LoadAsJson<Document>(options);
                if (document != null)
                    return document;
                else
                    throw new DatabaseLoadException(ErrorMessages.DOCUMENT_LOAD);
            } catch (DatabaseLoadException) {
                throw;
            } catch (JsonException){
                throw new DatabaseLoadException(ErrorMessages.DOCUMENT_LOAD);
            }
        }
    }
}