namespace DatabaseNS.ResultNS.Messages;

using System.Text;
using DatabaseNS.Components.IndexNS;
using DatabaseNS.Components.Values;

public class CorrectMessages {
    internal static Message DocumentAdded(ComponentName documentName) {
        return new Message(
            string.Format("Document '{0}' was added", documentName)
        );
    }
    internal static Message DocumentRemoved(ComponentName documentName) {
        return new Message(
            string.Format("Document '{0}' was successfully removed", documentName)
        );
    }
    internal static Message DocumentReturned(ComponentName documentName, string content) {
        return new Message(
            string.Format("Document '{0}': \n {1}", documentName, content)
        );
    }
    internal static Message CollectionCreated(ComponentName collectionName) {
        return new Message(
            string.Format("Collection '{0}' was created", collectionName)
        );
    }
    internal static Message CollectionDropped(ComponentName collectionName) {
        return new Message(
            string.Format("Collection '{0}' was deleted", collectionName)
        );
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
        return new Message(
            string.Format("Treshhold was set to '{0}'", newValue)
        );
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
        buffer.AppendLine($"Documents from collection '{collection}'");
        foreach(var document in documents) {
            buffer.AppendLine($"  - {document}");
        }
        return new Message(buffer.ToString());
    }

}