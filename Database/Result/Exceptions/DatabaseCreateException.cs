namespace DatabaseNS.ResultNS.Exceptions;

public class DatabaseCreateException : ValueException {
    public DatabaseCreateException(Result value) : base(value) {}
    public DatabaseCreateException(Result value, Exception cause) : base(value, cause) {}
}