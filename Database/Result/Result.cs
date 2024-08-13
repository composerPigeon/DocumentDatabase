namespace DatabaseNS.ResultNS;

using DatabaseNS.ResultNS.Messages;
using DatabaseNS.ResultNS.Handlers;

public struct Result {
    public Message Message { get; init; }
    public ResultType Type { get; init;}

    private Result(Message message, ResultType type) {
        Message = message;
        Type = type;
    }

    public override string ToString() {
        return Message.ToString();
    }

    internal static CorrectHandler GetResultHandler() {
        return new CorrectHandler(
            (message, type) => new Result(message, type)
        );
    }

    internal static ErrorHandler GetErrorHandler() {
        return new ErrorHandler(
            (message, type) => new Result(message, type)
        );
    }

    internal static ExceptionHandler GetExceptionHandler(ErrorHandler errorHandler) {
        return new ExceptionHandler(
            (message, type) => new Result(message, type),
            errorHandler
        );
    }
}

public enum ResultType {
    Ok,
    BadRequest,
    InternalServerError
}

public static class DatabaseResultTypeEtensions {
    public static string GetString(this ResultType type) {
        switch (type) {
            case ResultType.Ok:
                return "Ok";
            case ResultType.BadRequest:
                return "BadRequest";
            case ResultType.InternalServerError:
                return "InternalServerError";
            default:
                return "";
        }
    }
}