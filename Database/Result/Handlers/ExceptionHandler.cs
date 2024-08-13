namespace DatabaseNS.ResultNS.Handlers;

using DatabaseNS.ResultNS.Messages;
using DatabaseNS.ResultNS.Exceptions;
using DatabaseNS.Components.Values;
using DatabaseNS.Components;
using DatabaseNS.Tokenization;
using Microsoft.VisualBasic;

internal class ExceptionHandler : ResultHandler {

    private ErrorHandler _error;
    public ExceptionHandler(Func<Message, ResultType, Result> createValue, ErrorHandler errorHandler) : base(createValue) {
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

    // ====== Database load exceptions ======

    // ====== File system access exceptions ======

    public ResultException ThrowDocumentFileRemove(ComponentName documentName, Exception cause) {
        return new DatabaseException(
            InitResult(ErrorMessages.DocumentFileRemove(documentName), ResultType.InternalServerError),
            cause
        );
    }

    public ResultException ThrowDocumentFileRead(ComponentName documentName, Exception cause) {
        return new DatabaseException(
            InitResult(ErrorMessages.DocumentFileRead(documentName), ResultType.InternalServerError),
            cause
        );
    }

    public ResultException ThrowDocumentFileCreate(ComponentName documentName, Exception cause) {
        return new DatabaseException(
            InitResult(ErrorMessages.DocumentFileCreate(documentName), ResultType.InternalServerError),
            cause
        );
    }

    public ResultException ThrowCollectionDirectoryCreate(ComponentName collectionName, Exception cause) {
        return new DatabaseException(
            InitResult(ErrorMessages.CollectionDirectoryCreate(collectionName), ResultType.InternalServerError),
            cause
        );
    }

    public ResultException ThrowCollectionDirectoryRemove(ComponentName collectionName, Exception cause) {
        return new DatabaseException(
            InitResult(ErrorMessages.CollectionDirectoryRemove(collectionName), ResultType.InternalServerError),
            cause
        );
    }

    public ResultException ThrowComponentAsJsonSave(ComponentPath path, Exception cause) {
        return new DatabaseException(
            InitResult(ErrorMessages.ComponentAsJsonSave(path), ResultType.InternalServerError),
            cause
        );
    }

    public ResultException ThrowComponentFromJsonLoad(ComponentPath path, Exception cause) {
        return new DatabaseException(
            InitResult(ErrorMessages.ComponentFromJsonLoad(path), ResultType.InternalServerError),
            cause
        );
    }

}