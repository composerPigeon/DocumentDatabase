namespace DatabaseNS.ResultNS.Handlers;

using DatabaseNS.ResultNS.Messages;

internal abstract class ResultHandler {
    protected Func<Message, ResultType, Result> InitResult;
    protected ResultHandler(Func<Message, ResultType, Result> initResult) {
        InitResult = initResult;
    }
}

internal static class Handlers {
    public static ErrorHandler Error = ResultNS.Result.GetErrorHandler();
    public static CorrectHandler Result = ResultNS.Result.GetResultHandler();
}