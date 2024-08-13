namespace DatabaseNS.ResultNS.Handlers;

using DatabaseNS.Components;
using DatabaseNS.ResultNS.Messages;

internal abstract class ResultHandler {
    protected Func<Message, ResultType, Result> InitResult;
    protected ResultHandler(Func<Message, ResultType, Result> initResult) {
        InitResult = initResult;
    }

    protected string parseComponentTypeToString(ComponentType type) {
        switch (type) {
            case ComponentType.Database: return "database";
            case ComponentType.Collection: return "collection";
            case ComponentType.Index: return "index"; 
            case ComponentType.Document: return "document";
            case ComponentType.DocumentStats: return "document statistics";
            default: return "";
        }
    }
}

internal static class Handlers {
    public static ErrorHandler Error = ResultNS.Result.GetErrorHandler();
    public static CorrectHandler Result = ResultNS.Result.GetResultHandler();

    public static ExceptionHandler Exception = ResultNS.Result.GetExceptionHandler(Error);
}