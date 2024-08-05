namespace DatabaseNS.ResultNS.Handlers;

using DatabaseNS.FileSystem;
using DatabaseNS.Tokenization;
using DatabaseNS.ResultNS.Messages;
using DatabaseNS.ResultNS.Exceptions;
using DatabaseNS.Components;

internal class ErrorHandler : ResultHandler {
    public ErrorHandler(Func<Message, ResultType, Result> createValue) : base(createValue) {}

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

    //Command parse errors

    public Result HandleCommandParseShort() {
        return InitResult(ErrorMessages.CommandParseShort(), ResultType.BadRequest);
    }
    public ResultException ThrowCommandParseShort() {
        return new CommandParseException(HandleCommandParseShort());
    }
    public Result HandleCommandParseLong() {
        return InitResult(ErrorMessages.CommandParseLong(), ResultType.BadRequest);
    }
    public Result HandleCommandParseInvalidToken(Token token) {
        return InitResult(ErrorMessages.CommandParseInvalidToken(token), ResultType.BadRequest);
    }
    public ResultException ThrowCommandParseInvalidToken(Token token) {
        return new CommandParseException(HandleCommandParseInvalidToken(token));
    }

    public Result HandleCommandInvalid(string command) {
        return InitResult(ErrorMessages.CommandInvalid(command), ResultType.BadRequest);
    }
    public ResultException ThrowCommandInvalid(string command) {
        return new CommandParseException(HandleCommandInvalid(command));
    }
    public ResultException ThrowCommandParseInvalidState() {
        return new CommandParseException(
            InitResult(ErrorMessages.CommandParseInvalidState(), ResultType.InternalServerError)
        );
    }

    //Invalid values
    public Result HandleComponentNameInvalid(ComponentType type, ComponentName? name) {
        var strType = "";
        switch (type) {
            case ComponentType.Database: strType = "database"; break;
            case ComponentType.Collection: strType = "collection"; break;
            case ComponentType.Index: strType = "index"; break;
            case ComponentType.Document: strType = "document"; break;
            case ComponentType.DocumentStats: strType = "document statistics"; break;
        }
        return InitResult(ErrorMessages.ComponentNameInvalid(strType, name.HasValue ? name.Value : ComponentName.Empty), ResultType.BadRequest);
    }
    public Result HandleInvalidTreshholdValueFormat(string value) {
        return InitResult(ErrorMessages.TreshholdInvalidFormatValue(value), ResultType.BadRequest);
    }
    public Result HandleInvalidTreshholdInterval(double value) {
        return InitResult(ErrorMessages.TreshholdInvalidInterval(value), ResultType.BadRequest);
    }
    public ResultException ThrowQueryInvalid(int queryLen, int documenLen) {
        return new ResultException(
            InitResult(ErrorMessages.QueryInvalid(queryLen, documenLen), ResultType.InternalServerError)
        );
    }

    // Build errors
    public ResultException ThrowComponentNameInvalid(ComponentType type, ComponentName? name) {
        return new DatabaseException(
            HandleComponentNameInvalid(type, name)
        );
    }

    public ResultException ThrowDatabaseCreate() {
        return new DatabaseCreateException(
            InitResult(ErrorMessages.DatabaseCreate(), ResultType.InternalServerError)
        );
    }

    public ResultException ThrowDocumentCreate(ComponentName documentName) {
        return new DatabaseCreateException(
            InitResult(ErrorMessages.DocumentCreate(documentName), ResultType.InternalServerError)
        );
    }

    public ResultException ThrowDocumentStatsCreate(ComponentName documentName) {
        return new DatabaseCreateException(
            InitResult(ErrorMessages.StatsCreate(documentName), ResultType.InternalServerError)
        );
    }

    public ResultException ThrowIndexCreate(ComponentName indexName) {
        return new DatabaseCreateException(
            InitResult(ErrorMessages.IndexCreate(indexName), ResultType.InternalServerError)
        );
    }
    public ResultException ThrowCollectionCreate(ComponentName collectionName) {
        return new DatabaseCreateException(
            InitResult(ErrorMessages.CollectionCreate(collectionName), ResultType.InternalServerError)
        );
    }
}