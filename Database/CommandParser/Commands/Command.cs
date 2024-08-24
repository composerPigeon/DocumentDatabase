namespace DatabaseNS.CommandParserNS.Commands;

// base command class
internal class Command {
    public string Value { get; }
    public CommandType Type { get; }

    public Command(CommandType type, string cmd) {
        Type = type;
        Value = cmd;
    }

    private static Command empty = new Command(CommandType.Empty, "");

    public static Command Empty {
        get { return empty; }
    }
}

internal enum CommandType {
    Empty,
    Create,
    Delete,
    Find,
    Load,
    Treshhold,
    List
}