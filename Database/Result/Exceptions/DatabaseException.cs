namespace DatabaseNS.ResultNS.Exceptions;

// Result exception which is general
internal class DatabaseException : ResultException {
    public DatabaseException(Result result) : base(result) { }
    public DatabaseException(Result result, Exception cause) : base(result, cause) { }
}