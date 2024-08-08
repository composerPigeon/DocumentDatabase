using DatabaseNS.FileSystem;

namespace DatabaseNS.CommandParserNS.Commands;

internal class ContentCollectionCommand : CollectionCommand, IContentCommand {
    private string[] _content;
    string[] IContentCommand.Content {
        get {
            return _content; 
        }
    }
    public ContentCollectionCommand(ComponentName collection, string[] content, CommandType type) : base(collection, type) {
        _content = content;
    }
}