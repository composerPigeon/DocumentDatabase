namespace DatabaseNS.CommandParserNS.Commands;

using DatabaseNS.FileSystem;

internal class DocumentCommand : CollectionCommand {
    public ComponentName Document { get; }

    public DocumentCommand(ComponentName document, ComponentName collection, CommandType type) : base(collection, type) {
        Document = document;
    }
}