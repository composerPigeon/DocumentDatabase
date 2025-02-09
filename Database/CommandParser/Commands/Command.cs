namespace DatabaseNS.CommandParserNS.Commands;

// base command class
internal class Command(CommandType type, string cmd)
{
    public string Value { get; } = cmd;
    public CommandType Type { get; } = type;
    
    public static readonly Command Empty = new (CommandType.Empty, "");
}

internal enum CommandType {
    Empty,
    Create,
    Delete,
    Find,
    Load,
    Threshold,
    List
}