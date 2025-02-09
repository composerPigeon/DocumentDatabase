namespace DatabaseNS.CommandParserNS.Commands;

using DatabaseNS.Components.Values;

// command which operates with collection, but contains some additional content
internal class ContentCollectionCommand(ComponentName collection, string[] content, CommandType type, string strCmd) : CollectionCommand(collection, type, strCmd), IContentCommand
{
    public string[] Content { get; } = content;

    public bool TryGetString(int pos, out string value) {
        value = "";
        if (Content.Length > pos && pos >= 0) {
            value = Content[pos];
            return true;
        }
        return false;
    }

    public bool TryGetPath(int pos, out ComponentPath value) {
        value = "".AsPath();
        if (Content.Length > pos && pos >= 0) {
            value = Content[pos].AsPath();
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