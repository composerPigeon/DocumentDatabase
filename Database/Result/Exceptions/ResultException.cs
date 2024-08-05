namespace DatabaseNS.ResultNS.Exceptions;

public class ResultException : Exception {
    public Result Result { get; }

    public ResultException(Result result) : base(result.Message.ToString()) {
        Result = result;
    }
    public ResultException(Result result, Exception cause) : base(result.Message.ToString(), cause) {
        Result = result;
    }
}