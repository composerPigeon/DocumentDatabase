namespace DatabaseNS.ResultNS.Exceptions;

// exceptions which occurs during command parsing process
internal class CommandParseException : ResultException {
    public CommandParseException(Result value) : base(value) {}
    public CommandParseException(Result value, Exception cause) : base(value, cause) {}
}