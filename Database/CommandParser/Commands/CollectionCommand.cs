namespace DatabaseNS.CommandParserNS.Commands;

using DatabaseNS.Components.Values;

// Command which operates with collection
internal class CollectionCommand(ComponentName collection, CommandType type, string strCmd) : Command(type, strCmd)
{
    public ComponentName Collection { get; } = collection;
}