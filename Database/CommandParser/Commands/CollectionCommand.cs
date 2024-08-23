namespace DatabaseNS.CommandParserNS.Commands;

using DatabaseNS.Components.Values;

// Command which operates with collection
internal class CollectionCommand : Command {
    public ComponentName Collection {get;}
    public CollectionCommand(ComponentName collection, CommandType type, string strCmd) : base(type, strCmd) {
        Collection = collection;
    }
}