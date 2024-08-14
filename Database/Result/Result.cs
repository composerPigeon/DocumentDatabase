namespace DatabaseNS.ResultNS;

using System.Text;

using DatabaseNS.ResultNS.Messages;
using DatabaseNS.ResultNS.Handlers;

public struct Result {
    public Message Message { get; init;}
    public Exception? Cause { get; init;}
    public ResultType Type { get; init;}

    private Result(Message message, ResultType type) {
        Message = message;
        Type = type;
    }

    private Result(Message message, ResultType type, Exception cause) {
        Message = message;
        Type = type;
        Cause = cause;
    }

    public override string ToString() {
        var buffer = new StringBuilder();
        buffer.AppendLine("Result:");
        buffer.AppendLine($"  - Type: {Type}");
        buffer.AppendLine($"  - Message: {Message}");

        if (Cause != null) {
            buffer.AppendLine($"  - Cause: {Cause}");
        }

        return buffer.ToString();
    }

    internal static CorrectHandler GetResultHandler() {
        return new CorrectHandler(
            (message, type) => new Result(message, type),
            (message, type, cause) => new Result(message, type, cause)
        );
    }

    internal static ErrorHandler GetErrorHandler() {
        return new ErrorHandler(
            (message, type) => new Result(message, type),
            (message, type, cause) => new Result(message, type, cause)
        );
    }

    internal static ExceptionHandler GetExceptionHandler(ErrorHandler errorHandler) {
        return new ExceptionHandler(
            (message, type) => new Result(message, type),
            (message, type, cause) => new Result(message, type, cause),
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