namespace DatabaseNS.ResultNS;

using System.Text;

using DatabaseNS.ResultNS.Messages;
using DatabaseNS.ResultNS.Handlers;
using System.Text.Json;
using System.Text.Json.Serialization;

// strcut which represents the result of command processing
public struct Result {
    public Message Message { get; init;}
    // Information about exception causing some error
    public Dictionary<string, string?>? Cause { get; init;}
    public ResultType Type { get; init;}

    private Result(Message message, ResultType type) {
        Message = message;
        Type = type;
    }

    private Result(Message message, ResultType type, Exception cause) {
        Message = message;
        Type = type;
        Cause = new Dictionary<string, string?>{
            {"Message", cause.Message},
            {"StackTrace", cause.StackTrace},
            {"Type", cause.GetType().ToString()}
        };
    }

    // static methods for getting instance of specified handlers
    internal static CorrectHandler GetCorrectHandler() {
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

    private static JsonSerializerOptions _options = initOptions();

    private static JsonSerializerOptions initOptions() {
        var options = new JsonSerializerOptions() {
            WriteIndented = true
        };
        options.Converters.Add(new MessageJsonConverter());
        options.Converters.Add(new ResultTypeJsonConverter());
        return options;
    }

    // property for getting best JsonSerializerOptions when you want to serialize Result to Json
    public static JsonSerializerOptions JsonSerializerOptions {
        get {
            return _options;
        }
    }
}

// says if Result is correct or some error happend
public enum ResultType {
    Ok,
    BadRequest,
    InternalServerError
}

// extension method for getting string from ResultType
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

// Converter for converting ResultType to json as a string
public class ResultTypeJsonConverter : JsonConverter<ResultType>
{
    public override ResultType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        switch(reader.GetString()!) {
            case "Ok":
                return ResultType.Ok;
            case "BadRequest":
                return ResultType.BadRequest;
            default:
                return ResultType.InternalServerError;
        }
    }

    public override void Write(Utf8JsonWriter writer, ResultType value, JsonSerializerOptions options) {
        writer.WriteStringValue(value.GetString());
    }
}