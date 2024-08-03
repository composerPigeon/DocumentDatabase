namespace DatabaseNS.ResultNS.Handlers;

using DatabaseNS.FileSystem;
using DatabaseNS.ResultNS.Messages;

internal class CorrectHandler : ResultHandler {
    public CorrectHandler(InitValueDelegate createValue) : base(createValue) {}

    public Result HandleDocumentAdded(ComponentName documentName) {
        return InitValue(CorrectMessages.DocumentAdded(documentName), ValueType.Ok);
    }

    public Result HandleDocumentRemoved(ComponentName documentName) {
        return InitValue(CorrectMessages.DocumentRemoved(documentName), ValueType.Ok);
    }

    public Result HandleDocumentReturned(ComponentName documentName, string content) {
        return InitValue(CorrectMessages.DocumentReturned(documentName, content), ValueType.Ok);
    }

    public Result HandleCollectionCreated(ComponentName collectionName) {
        return InitValue(CorrectMessages.CollectionCreated(collectionName), ValueType.Ok);
    }
    public Result HandleCollectionDropped(ComponentName collectionName) {
        return InitValue(CorrectMessages.CollectionDropped(collectionName), ValueType.Ok);
    }

    public Result HandleQueryResult(ComponentName collectionName, string query) {
        return InitValue(CorrectMessages.QueryResult(collectionName, query), ValueType.Ok);
    }
}