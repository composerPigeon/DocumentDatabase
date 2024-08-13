namespace DatabaseNS.CommandParserNS.Commands;

using DatabaseNS.Components.Values;

internal class DocumentCommand : CollectionCommand {
    public ComponentName Document { get; }

    public DocumentCommand(ComponentName document, ComponentName collection, CommandType type, string strCmd) : base(collection, type, strCmd) {
        Document = document;
    }
}