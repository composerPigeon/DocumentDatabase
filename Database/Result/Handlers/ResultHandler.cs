namespace DatabaseNS.ResultNS.Handlers;

using DatabaseNS.Components;
using DatabaseNS.ResultNS.Messages;

// Abstract class for handling results and errors (works like a factory for Result type)
internal abstract class ResultHandler {
    protected Func<Message, ResultType, Result> InitResult;
    protected Func<Message, ResultType, Exception, Result> InitResultWithException;

    // Need functions for constructing Result (best way of inicializing handler is using static methods of DatabaseNS.ResultNS.Result)
    protected ResultHandler(
        Func<Message, ResultType, Result> initResult,
        Func<Message, ResultType, Exception, Result> initResultWithException
    ) {
        InitResult = initResult;
        InitResultWithException = initResultWithException;
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
    public static CorrectHandler Result = ResultNS.Result.GetCorrectHandler();

    public static ExceptionHandler Exception = ResultNS.Result.GetExceptionHandler(Error);
}