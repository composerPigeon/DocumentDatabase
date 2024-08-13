namespace DatabaseNS.CommandParserNS.Commands;

using DatabaseNS.Components.Values;

internal class ContentCollectionCommand : CollectionCommand, IContentCommand {
    private string[] _content;
    public ContentCollectionCommand(ComponentName collection, string[] content, CommandType type, string strCmd) : base(collection, type, strCmd) {
        _content = content;
    }

    public string[] Content { get { return _content; } }

    public bool TryGetString(int pos, out string value) {
        value = "";
        if (Content.Length > pos && pos >= 0) {
            value = Content[pos];
            return true;
        }
        return false;
    }
    
    public bool TryGetDouble(int pos, out double value) {
        value = 0;
        if (Content.Length > pos && pos >= 0) {
            if (double.TryParse(Content[pos], out value)) {
                return true;
            }
        }
        return false;
    }
}