namespace DatabaseNS.ResultNS.Messages;

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
    internal static Message QueryResult(ComponentName collectionName, string result) {
        return new Message(
            string.Format("Result for collection '{0}': \n{1}", collectionName, result)
        );
    }
    internal static Message TreshholdSet(double newValue) {
        return new Message(
            string.Format("Treshhold was set to '{0}'", newValue)
        );
    }

    public static Message FromString(string message) {
        return new Message(message);
    }
}