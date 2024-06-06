using System.Dynamic;

namespace Database_DbComponents;

public struct Result {
    public string Message { get; init; }
    public Action<int>? Action{ get; init; }

    public Result(string message) {
        Message = message;
        Action = null;
    }

    public Result(string message, Action<int>? action) {
        Message = message;
        Action = action;
    }

    public override string ToString() {
        return Message;
    }
}