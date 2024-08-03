namespace DatabaseNS.ResultNS.Handlers;

using DatabaseNS.FileSystem;
using DatabaseNS.Tokenization;
using DatabaseNS.ResultNS.Messages;
using DatabaseNS.ResultNS.Exceptions;

internal class ErrorHandler : ResultHandler {
    public ErrorHandler(InitValueDelegate createValue) : base(createValue) {}

    public Result HandleDocumentMissing(ComponentName name) {
        return InitValue(ErrorMessages.DocumentMissing(name), ValueType.BadRequest);
    }
    public Result HandleDocumentNotFound(ComponentName name) {
        return InitValue(ErrorMessages.DocumentMissing(name), ValueType.NotFound);
    }
    public Result HandleDocumentExists(ComponentName documentName) {
        return InitValue(ErrorMessages.DocumentExists(documentName), ValueType.BadRequest);
    }
    public Result HandleCollectionMissing(ComponentName collectionName) {
        return InitValue(ErrorMessages.CollectionMissing(collectionName), ValueType.BadRequest);
    }
    public Result HandleCollectionExists(ComponentName collectionName) {
        return InitValue(ErrorMessages.CollectionExists(collectionName), ValueType.BadRequest);
    }
    public Result HandleComponentNameMissing() {
        return InitValue(ErrorMessages.ComponentNameMissing(), ValueType.BadRequest);
    }

    //Command parse errors

    public Result HandleCommandParseShort() {
        return InitValue(ErrorMessages.CommandParseShort(), ValueType.BadRequest);
    }
    public ValueException ThrowCommandParseShort() {
        return new CommandParseException(HandleCommandParseShort());
    }
    public Result HandleCommandParseLong() {
        return InitValue(ErrorMessages.CommandParseLong(), ValueType.BadRequest);
    }
    public Result HandleCommandParseInvalidToken(Token token) {
        return InitValue(ErrorMessages.CommandParseInvalidToken(token), ValueType.BadRequest);
    }
    public ValueException ThrowCommandParseInvalidToken(Token token) {
        return new ValueException(HandleCommandParseInvalidToken(token));
    }

    public Result HandleCommandInvalid(string command) {
        return InitValue(ErrorMessages.CommandInvalid(command), ValueType.BadRequest);
    }
    public ValueException ThrowCommandInvalid(string command) {
        return new CommandParseException(HandleCommandInvalid(command));
    }
    public ValueException ThrowCommandParseInvalidState() {
        return new CommandParseException(
            InitValue(ErrorMessages.CommandParseInvalidState(), ValueType.InternalServerError)
        );
    }

    //INvalid values
    public Result HandleInvalidName(ComponentName name) {
        return InitValue(ErrorMessages.NameInvalid(name), ValueType.BadRequest);
    }
    public Result HandleInvalidTreshholdValueFormat(string value) {
        return InitValue(ErrorMessages.TreshholdInvalidFormatValue(value), ValueType.BadRequest);
    }
    public Result HandleInvalidQuery(string query) {
        return InitValue(ErrorMessages.QueryInvalid(query), ValueType.BadRequest);
    }

    //Load errors
    public static Message IndexLoad(ComponentName collectionName) {
        return new Message(
            string.Format("Index for collection '{0}' couldn't be loaded.", collectionName)
        );
    }
    public static Message StatsLoad(ComponentName collectionName, ComponentName documentName) {
        return new Message(
            string.Format("Document statistics '{0}' from the collection '{1}' couldn't be loaded.", documentName, collectionName)
        );
    }

    // Build errors
    public ValueException ThrowDatabaseCreate() {
        return new DatabaseCreateException(
            InitValue(ErrorMessages.DatabaseCreate(), ValueType.InternalServerError)
        );
    }

    public ValueException ThrowDocumentCreate(ComponentName collectionName, ComponentName documentName) {
        return new DatabaseCreateException(
            InitValue(ErrorMessages.DocumentCreate(collectionName, documentName), ValueType.InternalServerError)
        );
    }

    public ValueException ThrowDocumentStatsCreate(ComponentName collectionName, ComponentName documentName) {
        return new DatabaseCreateException(
            InitValue(ErrorMessages.StatsCreate(collectionName, documentName), ValueType.InternalServerError)
        );
    }

    public ValueException ThrowIndexCreate(ComponentName collectionName) {
        return new DatabaseCreateException(
            InitValue(ErrorMessages.IndexCreate(collectionName), ValueType.InternalServerError)
        );
    }
    public ValueException ThrowCollectionCreate(ComponentName collectionName) {
        return new ValueException(
            InitValue(ErrorMessages.CollectionCreate(collectionName), ValueType.InternalServerError)
        );
    }
}