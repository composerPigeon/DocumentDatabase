namespace DatabaseNS.FileSystem;

using System.Text.Json;

using DatabaseNS.Components;
using DatabaseNS.Components.IndexNS;
using DatabaseNS.Components.Values;
using DatabaseNS.Components.Builders;
using DatabaseNS.ResultNS.Handlers;

internal static class FileSystemAccessHandler {

    private static JsonSerializerOptions jsonSerializerOptions = initOptions();

    private static readonly ComponentPath INDEX_DIR = ".index".AsPath();
    public static readonly ComponentPath DATA_DIR = "data".AsPath();

    private static JsonSerializerOptions initOptions() {
        var options = new JsonSerializerOptions {
            WriteIndented = true,
        };
        options.Converters.Add(new NameToStringAsPropertyConverter());
        return options;
    }

    private static IEnumerable<ComponentPath> listDirectory(ComponentPath directory) {
        foreach (string stringPath in Directory.EnumerateFileSystemEntries(directory)) {
            yield return stringPath.AsPath();
        }
    }

    private static void writeAsJson<TComponent>(TComponent component)
        where TComponent : DatabaseComponent
    {
        try {
            string content = JsonSerializer.Serialize(component);
            File.WriteAllText(component.Path, content);
        } catch (Exception e) when (e is JsonException || e is DirectoryNotFoundException || e is UnauthorizedAccessException) {
            throw Handlers.Exception.ThrowComponentAsJsonSave(component.Path, e);
        } 
    }

    public static ComponentPath GetIndexDirectoryPath(ComponentPath collectionPath) {
        return collectionPath.AppendPath(INDEX_DIR);
    }

    private static ComponentPath getIndexFilePath(ComponentPath collectionPath) {
        return collectionPath.AppendPath(INDEX_DIR).AppendString("index.json");
    }

    public static void SaveIndex(Index index) {
        writeAsJson(index);
    }

    public static void AddCollection(Collection collection) {
        try {
            Directory.CreateDirectory(collection.Path);
        } catch (Exception e) when (e is IOException || e is UnauthorizedAccessException || e is DirectoryNotFoundException) {
            Handlers.Exception.ThrowCollectionDirectoryCreate(collection.Name, e);
        }
    }

    public static void RemoveCollection(Collection collection) {
        try {
            Directory.Delete(collection.Path, true);
        } catch (Exception e) when ( e is DirectoryNotFoundException || e is UnauthorizedAccessException) {
            Handlers.Exception.ThrowCollectionDirectoryRemove(collection.Name, e);
        }   
    }

    // Creates files in file system for document and its statistics
    public static void AddDocument(Document document, string content) {
        try {
            File.WriteAllText(document.Path, content);
            writeAsJson(document.Stats);
        } catch (Exception e) when (e is DirectoryNotFoundException || e is UnauthorizedAccessException) {
            throw Handlers.Exception.ThrowDocumentFileCreate(document.Name, e);
        }
    }

    public static void RemoveDocument(Document document) {
        try {
            File.Delete(document.Path);
            File.Delete(document.Stats.Path);
        } catch (Exception e) when (e is UnauthorizedAccessException || e is DirectoryNotFoundException) {
            throw Handlers.Exception.ThrowDocumentFileRemove(document.Name, e);
        }
    }

    public static string ReadDocument(Document document) {
        try {
            return File.ReadAllText(document.Path);
        } catch (Exception e) when (e is FileLoadException || e is UnauthorizedAccessException || e is FileNotFoundException) {
            throw Handlers.Exception.ThrowDocumentFileRead(document.Name, e);
        }
    }


    // ====== Load Section =======

    // help function which loads json content from file and parse its content into instance of DatabaseComponent
    private static TComponent loadFromJson<TComponent>(ComponentPath path)
        where TComponent : DatabaseComponent
    {
        try {
            string content = File.ReadAllText(path);
            TComponent? component = JsonSerializer.Deserialize<TComponent>(content, jsonSerializerOptions);
            if (component == null) {
                throw new JsonException("Component was returned as null");
            }
            return component;
        } catch (Exception e) when (e is JsonException || e is DirectoryNotFoundException || e is FileNotFoundException || e is IOException || e is UnauthorizedAccessException) {
            throw Handlers.Exception.ThrowComponentFromJsonLoad(path, e);
        }
    }

    public static Database LoadDatabase() {
        DatabaseBuilder builder = Database.CreateBuilder();
        builder.Path = DATA_DIR;
        builder.Collections = loadCollections(DATA_DIR);
        return builder.Build();
    }

    // Iterates through data directory and load each subdirectory as collection
    private static Dictionary<ComponentName, Collection> loadCollections(ComponentPath path) {
        var collections = new Dictionary<ComponentName, Collection>();
        var options = new JsonSerializerOptions() {
            WriteIndented = true,
        };
        options.Converters.Add(new NameToStringAsPropertyConverter());

        foreach (var collectionPath in listDirectory(path)) {
            Collection collection = loadCollection(collectionPath);
            collections.Add(collection.Name, collection);
        }
        return collections;
    }

    //Loads directory as a collection
    private static Collection loadCollection(ComponentPath collectionPath) {
        var builder = Collection.CreateBuilder();
        builder.Documents = new Dictionary<ComponentName, Document>();
        builder.Index = loadFromJson<Index>(getIndexFilePath(collectionPath));

        foreach(var componentPath in listDirectory(collectionPath)) {
            if (componentPath.EndsWith(".txt")) {
                ComponentName documentName = componentPath.GetComponentName();
                Document document = loadDocument(collectionPath, documentName);
                builder.Documents.Add(document.Name, document);
            }
        }
        return builder.Build();
    }

    // For each plain text file in collection directory tries to find its statistics and then load those pairs as document
    private static Document loadDocument(ComponentPath collectionPath, ComponentName documentName) {
        ComponentPath documentStatsPath = GetIndexDirectoryPath(collectionPath).AppendPath(documentName.WithExtension(".json"));
        DocumentStats stats = loadFromJson<DocumentStats>(documentStatsPath);

        ComponentPath documentPath = collectionPath.AppendPath(documentName.WithExtension(".txt"));
        
        var builder = Document.CreateBuilder();
        builder.Name = documentName;
        builder.Path = documentPath;
        builder.Stats = stats;
        return builder.Build();
    }
}