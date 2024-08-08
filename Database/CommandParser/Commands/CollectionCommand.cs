namespace DatabaseNS.CommandParserNS.Commands;

using DatabaseNS.FileSystem;

internal class CollectionCommand : Command {
    public ComponentName Collection {get;}
    public CollectionCommand(ComponentName collection, CommandType type) : base(type) {
        Collection = collection;
    }
}