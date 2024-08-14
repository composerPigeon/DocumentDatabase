namespace DatabaseNS.ResultNS.Handlers;

using System.Text;
using DatabaseNS.Components.IndexNS;
using DatabaseNS.Components.Values;
using DatabaseNS.ResultNS.Messages;

internal class CorrectHandler : ResultHandler {
    public CorrectHandler(Func<Message, ResultType, Result> createValue) : base(createValue) {}

    public Result HandleDocumentAdded(ComponentName documentName) {
        return InitResult(CorrectMessages.DocumentAdded(documentName), ResultType.Ok);
    }

    public Result HandleDocumentRemoved(ComponentName documentName) {
        return InitResult(CorrectMessages.DocumentRemoved(documentName), ResultType.Ok);
    }

    public Result HandleDocumentReturned(ComponentName documentName, string content) {
        return InitResult(CorrectMessages.DocumentReturned(documentName, content), ResultType.Ok);
    }

    public Result HandleCollectionCreated(ComponentName collectionName) {
        return InitResult(CorrectMessages.CollectionCreated(collectionName), ResultType.Ok);
    }
    public Result HandleCollectionDropped(ComponentName collectionName) {
        return InitResult(CorrectMessages.CollectionDropped(collectionName), ResultType.Ok);
    }
    public Result HandleQueryResult(ComponentName collectionName, IEnumerable<IndexRecord> result) {
        return InitResult(CorrectMessages.QueryResult(collectionName, result), ResultType.Ok);
    }
    public Result HandleTreshholdSet(double newValue) {
        return InitResult(CorrectMessages.TreshholdSet(newValue), ResultType.Ok);
    }
    public Result HandleListCollections(IEnumerable<ComponentName> collections) {
        return InitResult(CorrectMessages.ListCollections(collections), ResultType.Ok);
    }

    public Result HandleListDocuments(ComponentName collection, IEnumerable<ComponentName> documents) {
        return InitResult(CorrectMessages.ListDocuments(collection, documents), ResultType.Ok);
    }
}