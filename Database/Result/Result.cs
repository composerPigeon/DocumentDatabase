namespace DatabaseNS.ResultNS;

using DatabaseNS.ResultNS.Messages;
using DatabaseNS.ResultNS.Handlers;

public struct Result {
    public Message Message { get; init; }
    public ValueType Type { get; init;}

    private Result(Message message, ValueType type) {
        Message = message;
        Type = type;
    }

    public override string ToString() {
        return Message.ToString();
    }

    internal static CorrectHandler GetResultHandler() {
        return new CorrectHandler(
            new InitValueDelegate((message, type) => new Result(message, type))
        );
    }

    internal static ErrorHandler GetErrorHandler() {
        return new ErrorHandler(
            new InitValueDelegate((message, type) => new Result(message, type))
        );
    }
}

public enum ValueType {
    Ok,
    BadRequest,
    NotFound,
    InternalServerError
}

public static class DatabaseResultTypeEtensions {
    public static string GetString(this ValueType type) {
        switch (type) {
            case ValueType.Ok:
                return "Ok";
            case ValueType.BadRequest:
                return "BadRequest";
            case ValueType.NotFound:
                return "NotFound";
            case ValueType.InternalServerError:
                return "InternalServerError";
            default:
                return "";
        }
    }
}