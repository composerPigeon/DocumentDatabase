namespace DatabaseNS.ResultNS.Exceptions;

public class ValueException : Exception {
    public Result Value { get; }

    public ValueException(Result value) : base(value.Message.ToString()) {
        Value = value;
    }
    public ValueException(Result value, Exception cause) : base(value.Message.ToString(), cause) {
        Value = value;
    }
}