namespace DatabaseNS.ResultNS.Messages;

using System.Text;
using DatabaseNS.Components.IndexNS;
using DatabaseNS.Components.Values;

// Contains all messages for correct results
public class CorrectMessages {
    internal static Message DocumentAdded(ComponentName documentName) {
        return new Message($"Document '{documentName}' was added");
    }
    internal static Message DocumentRemoved(ComponentName documentName) {
        return new Message($"Document '{documentName}' was successfully removed");
    }
    internal static Message DocumentReturned(ComponentName documentName, string content) {
        return new Message($"Document '{documentName}': {Environment.NewLine} {content}");
    }
    internal static Message CollectionCreated(ComponentName collectionName) {
        return new Message($"Collection '{collectionName}' was created");
    }
    internal static Message CollectionDropped(ComponentName collectionName) {
        return new Message($"Collection '{collectionName}' was deleted");
    }
    internal static Message QueryResult(ComponentName collectionName, IEnumerable<IndexRecord> result) {
        var buffer = new StringBuilder("Result for collection '");
        buffer.Append(collectionName).AppendLine("':");
        foreach (var record in result) {
            buffer.AppendLine($"  - {record.Score.ToString("F")}\t{record.DocumentName}");
        }
        return new Message(buffer.ToString());
    }
    internal static Message TreshholdSet(double newValue) {
        return new Message($"Treshhold was set to '{newValue:F}'");
    }

    internal static Message ListCollections(IEnumerable<ComponentName> collections) {
        var buffer = new StringBuilder();
        buffer.AppendLine("Collections:");
        foreach(var collection in collections) {
            buffer.AppendLine($"  -  {collection}");
        }
        return new Message(buffer.ToString());
    }

    internal static Message ListDocuments(ComponentName collection, IEnumerable<ComponentName> documents) {
        var buffer = new StringBuilder();
        buffer.AppendLine($"Documents in collection '{collection}':");
        foreach(var document in documents) {
            buffer.AppendLine($"  - {document}");
        }
        return new Message(buffer.ToString());
    }

    internal static Message DocumentsLoaded(ComponentName collection) {
        return new Message($"All of the documents were loaded to collection '{collection}'.");
    }

}