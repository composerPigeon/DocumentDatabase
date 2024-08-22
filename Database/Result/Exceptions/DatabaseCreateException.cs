namespace DatabaseNS.ResultNS.Exceptions;

// exception which occurs during creating of some component
internal class DatabaseCreateException : DatabaseException {
    public DatabaseCreateException(Result result) : base(result) {}
    public DatabaseCreateException(Result result, Exception cause) : base(result, cause) {}
}