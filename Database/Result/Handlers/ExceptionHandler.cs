namespace DatabaseNS.ResultNS.Handlers;

using DatabaseNS.ResultNS.Messages;
using DatabaseNS.ResultNS.Exceptions;
using DatabaseNS.Components.Values;
using DatabaseNS.Components;
using DatabaseNS.Tokenization;

// class which handles exceptions (factory for ResultExceptions)
internal class ExceptionHandler : ResultHandler {

    private ErrorHandler _error;
    public ExceptionHandler(
        Func<Message, ResultType, Result> initResult,
        Func<Message, ResultType, Exception, Result> initResultWithException,
        ErrorHandler errorHandler
    ) : base(initResult, initResultWithException) {
        _error = errorHandler;
    }

    // ====== Command parse exceptions ======

    public ResultException ThrowCommandInvalid(string command) {
        return new CommandParseException(
            _error.HandleCommandInvalid(command)
        );
    }
    public ResultException ThrowCommandParseInvalidState() {
        return new CommandParseException(
            InitResult(ErrorMessages.CommandParseInvalidState(), ResultType.InternalServerError)
        );
    }
    public ResultException ThrowCommandParseInvalidToken(Token token) {
        return new CommandParseException(
            InitResult(ErrorMessages.CommandParseInvalidToken(token), ResultType.BadRequest)
        );
    }

    public ResultException ThrowCommandParseCommandEmpty() {
        return new CommandParseException(
            InitResult(ErrorMessages.CommandParseCommandEmpty(), ResultType.BadRequest)
        );
    }

    // ====== Invalid query ======
    
    public ResultException ThrowQueryInvalid(int queryLen, int documenLen) {
        return new DatabaseException(
            InitResult(ErrorMessages.QueryInvalid(queryLen, documenLen), ResultType.InternalServerError)
        );
    }

    // ====== Component create exceptions ======

    public ResultException ThrowComponentNameInvalid(ComponentType type, ComponentName? name) {
        return new DatabaseException(
           _error.HandleComponentNameInvalid(type, name)
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

    // ====== File system access exceptions ======

    public ResultException ThrowDocumentFileRemove(ComponentName documentName, Exception cause) {
        return new DatabaseException(
            InitResultWithException(ErrorMessages.DocumentFileRemove(documentName), ResultType.InternalServerError, cause),
            cause
        );
    }

    public ResultException ThrowDocumentFileRead(ComponentName documentName, Exception cause) {
        return new DatabaseException(
            InitResultWithException(ErrorMessages.DocumentFileRead(documentName), ResultType.InternalServerError, cause),
            cause
        );
    }

    public ResultException ThrowDocumentFileCreate(ComponentName documentName, Exception cause) {
        return new DatabaseException(
            InitResultWithException(ErrorMessages.DocumentFileCreate(documentName), ResultType.InternalServerError, cause),
            cause
        );
    }

    public ResultException ThrowCollectionDirectoryCreate(ComponentName collectionName, Exception cause) {
        return new DatabaseException(
            InitResultWithException(ErrorMessages.CollectionDirectoryCreate(collectionName), ResultType.InternalServerError, cause),
            cause
        );
    }

    public ResultException ThrowCollectionDirectoryRemove(ComponentName collectionName, Exception cause) {
        return new DatabaseException(
            InitResultWithException(ErrorMessages.CollectionDirectoryRemove(collectionName), ResultType.InternalServerError, cause),
            cause
        );
    }

    public ResultException ThrowIndexDirectoryCreate(ComponentName indexName, Exception cause) {
        return new DatabaseException(
            InitResultWithException(ErrorMessages.IndexDirectoryCreate(indexName), ResultType.InternalServerError, cause),
            cause
        );
    }

    public ResultException ThrowComponentAsJsonSave(ComponentPath path, Exception cause) {
        return new DatabaseException(
            InitResultWithException(ErrorMessages.ComponentAsJsonSave(path), ResultType.InternalServerError, cause),
            cause
        );
    }

    public ResultException ThrowComponentFromJsonLoad(ComponentPath path, Exception cause) {
        return new DatabaseException(
            InitResultWithException(ErrorMessages.ComponentFromJsonLoad(path), ResultType.InternalServerError, cause),
            cause
        );
    }

    public ResultException ThrowFileRead(ComponentPath path, Exception cause) {
        return new DatabaseException(
            InitResultWithException(ErrorMessages.FileRead(path), ResultType.InternalServerError, cause),
            cause
        );
    }

}