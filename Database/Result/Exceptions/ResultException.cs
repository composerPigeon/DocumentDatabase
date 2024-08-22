namespace DatabaseNS.ResultNS.Exceptions;

// Class which describes exception that is able to hold Result with it. Does not have any name context so it is abstract
public abstract class ResultException : Exception {
    public Result Result { get; }

    public ResultException(Result result) : base(result.Message.ToString()) {
        Result = result;
    }
    public ResultException(Result result, Exception cause) : base(result.Message.ToString(), cause) {
        Result = result;
    }
}