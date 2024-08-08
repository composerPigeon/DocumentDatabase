using DatabaseNS.FileSystem;

namespace DatabaseNS.CommandParserNS.Commands;

internal class Command {
    public CommandType Type { get; }

    public Command(CommandType type) {
        Type = type;
    }

    private static Command empty = new CollectionCommand(ComponentName.Empty, CommandType.Empty);

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
    List,
    Save
}