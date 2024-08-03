namespace DatabaseNS.ResultNS.Exceptions;

internal class CommandParseException : ValueException {
    public CommandParseException(Result value) : base(value) {}
    public CommandParseException(Result value, Exception cause) : base(value, cause) {}
}