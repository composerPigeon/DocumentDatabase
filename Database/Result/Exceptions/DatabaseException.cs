namespace DatabaseNS.ResultNS.Exceptions;

internal class DatabaseException : ResultException {
    public DatabaseException(Result result) : base(result) { }
    public DatabaseException(Result result, Exception cause) : base(result, cause) { }
}