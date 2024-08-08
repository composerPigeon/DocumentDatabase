namespace DatabaseNS.CommandParserNS.Commands;

using DatabaseNS.FileSystem;

internal class ContentDocumentCommand : DocumentCommand, IContentCommand {
    private string[] _content;
    string[] IContentCommand.Content {
        get {
            return _content;
        }
    }

    public ContentDocumentCommand(ComponentName document, ComponentName collection, string[] content, CommandType type) : base(document, collection, type) {
        _content = content;
    }
}