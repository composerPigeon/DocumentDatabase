namespace DatabaseNS.Components;

using DatabaseNS.Components.Builders;
using DatabaseNS.Components.IndexNS;
using DocumentParserNS;
using System.Text.Json;

internal class DatabaseLoadException : Exception {
    public DatabaseLoadException(string message) : base(message) { }
}

internal class DatabaseLoader {

    public static readonly ComponentPath INDEX_DIRECTORY = "index".AsPath();

    public static Document AddNewDocument(ComponentName documentName, ComponentPath collectionPath, string content) {
        DocumentStats stats = DocumentStats.ReadDocument(
            documentName.Concat("_stats"),
            collectionPath + INDEX_DIRECTORY + documentName.WithExtension(".json"),
            content
        );

        DocumentBuilder builder = new DocumentBuilder() {
            Name = documentName,
            Path = collectionPath + documentName.WithExtension(".txt"),
            Stats = stats
        };

        return Document.BuildFrom(builder);
    }

    public Database Load(ComponentPath path) {
        DatabaseBuilder builder = new DatabaseBuilder() {
            Path = path,
            Collections = loadCollections(path)
        };
        return Database.BuildFrom(builder);
    }

    private static Dictionary<ComponentName, Collection> loadCollections(ComponentPath path) {
            var collections = new Dictionary<ComponentName, Collection>();
            var options = new JsonSerializerOptions() {
                WriteIndented = true,
            };
            options.Converters.Add(new NameToStringAsPropertyConverter());

            foreach (var collectionPath in path.List()) {
                if (!collectionPath.EndsWith("stopWords.json")) {
                    Collection collection = loadCollection(collectionPath, options);
                    collections.Add(collection.Name, collection);
                }
            }
            return collections;
        }

        private static Collection loadCollection(ComponentPath path, JsonSerializerOptions options) {
            var builder = new CollectionBuilder() {
                Documents = new Dictionary<ComponentName, Document>(),
                Index = loadIndex(path + INDEX_DIRECTORY + "index.json".AsPath(), options)
            };

            foreach(var componentPath in path.List()) {
                if (componentPath.EndsWith("index")) {
                    builder.Index = loadIndex(componentPath, options);
                } else if (componentPath.EndsWith(".txt")) {
                    ComponentName documentName = componentPath.GetComponentName();
                    Document document = loadDocument(path, documentName, options);
                    builder.Documents.Add(document.Name, document);
                }
            }
            return Collection.BuildFrom(builder);
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

        private static Document loadDocument(ComponentPath collectionPath, ComponentName documentName, JsonSerializerOptions options) {
            ComponentPath documentStatsPath = collectionPath + INDEX_DIRECTORY + documentName.WithExtension(".json");
            DocumentStats? stats = loadDocumentStats(documentStatsPath, options);

            ComponentPath documentPath = collectionPath + documentName.WithExtension(".txt");
            if (stats == null) {
                string documentContent = documentPath.GetReader().ReadToEnd();
                stats = DocumentStats.ReadDocument(documentName, documentPath, documentContent);
            }

            if (stats != null) {
                return Document.BuildFrom(
                    new DocumentBuilder() {
                        Name = documentName,
                        Path = documentPath,
                        Stats = stats
                    }
                );
            } else
                throw new DatabaseLoadException(ErrorMessages.STATS_LOAD);
        }

        private static DocumentStats? loadDocumentStats(ComponentPath path, JsonSerializerOptions options) {
            try {
                DocumentStats? document = path.LoadAsJson<DocumentStats>(options);
                return document;
            } catch (DatabaseLoadException) {
                throw;
            } catch (JsonException){
                throw new DatabaseLoadException(ErrorMessages.STATS_LOAD);
            }
        }
}