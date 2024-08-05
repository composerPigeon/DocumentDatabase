namespace DatabaseNS.ResultNS.Exceptions;

internal class DatabaseCreateException : DatabaseException {
    public DatabaseCreateException(Result result) : base(result) {}
    public DatabaseCreateException(Result result, Exception cause) : base(result, cause) {}
}