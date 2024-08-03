namespace DatabaseNS.ResultNS.Handlers;

using DatabaseNS.ResultNS.Messages;

internal delegate Result InitValueDelegate(Message message, ValueType type);

internal abstract class ResultHandler {
    protected InitValueDelegate InitValue;
    protected ResultHandler(InitValueDelegate initValue) {
        InitValue = initValue;
    }
}

internal static class Handlers {
    public static ErrorHandler Error = ResultNS.Result.GetErrorHandler();
    public static CorrectHandler Result = ResultNS.Result.GetResultHandler();
}