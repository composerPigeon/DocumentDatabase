namespace DatabaseNS.ResultNS.Handlers;

using DatabaseNS.Components.Values;
using DatabaseNS.Tokenization;
using DatabaseNS.ResultNS.Messages;
using DatabaseNS.ResultNS.Exceptions;
using DatabaseNS.Components;

internal class ErrorHandler : ResultHandler {
    public ErrorHandler(Func<Message, ResultType, Result> createValue) : base(createValue) {}

    // ====== Component errors ======

    public Result HandleDocumentMissing(ComponentName name) {
        return InitResult(ErrorMessages.DocumentMissing(name), ResultType.BadRequest);
    }
    public Result HandleDocumentExists(ComponentName documentName) {
        return InitResult(ErrorMessages.DocumentExists(documentName), ResultType.BadRequest);
    }
    public Result HandleCollectionMissing(ComponentName collectionName) {
        return InitResult(ErrorMessages.CollectionMissing(collectionName), ResultType.BadRequest);
    }
    public Result HandleCollectionExists(ComponentName collectionName) {
        return InitResult(ErrorMessages.CollectionExists(collectionName), ResultType.BadRequest);
    }
    public Result HandleComponentNameMissing() {
        return InitResult(ErrorMessages.ComponentNameMissing(), ResultType.BadRequest);
    }
    public Result HandleDocumentsSomeExisted(ComponentName collection) {
        return InitResult(ErrorMessages.SomeDocumentExisted(collection), ResultType.BadRequest);
    }

    // ====== Command parse errors ======

    public Result HandleCommandInvalid(string command) {
        return InitResult(ErrorMessages.CommandInvalid(command), ResultType.BadRequest);
    }

    // ====== Invalid values ======
    public Result HandleComponentNameInvalid(ComponentType type, ComponentName? name) {
        var strType = parseComponentTypeToString(type);
        return InitResult(ErrorMessages.ComponentNameInvalid(strType, name.HasValue ? name.Value : ComponentName.Empty), ResultType.BadRequest);
    }
    public Result HandleInvalidTreshholdValueFormat(string value) {
        return InitResult(ErrorMessages.TreshholdInvalidFormatValue(value), ResultType.BadRequest);
    }
    public Result HandleInvalidTreshholdInterval(double value) {
        return InitResult(ErrorMessages.TreshholdInvalidInterval(value), ResultType.BadRequest);
    }
    
}