namespace DatabaseNS.CommandParserNS.Commands;

using DatabaseNS.Components.Values;

// command which operates with documents
internal class DocumentCommand(ComponentName document, ComponentName collection, CommandType type, string strCmd)
    : CollectionCommand(collection, type, strCmd)
{
    public ComponentName Document { get; } = document;
}