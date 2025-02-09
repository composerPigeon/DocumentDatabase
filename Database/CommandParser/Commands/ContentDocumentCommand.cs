using System.Diagnostics;

namespace DatabaseNS.CommandParserNS.Commands;

using DatabaseNS.Components.Values;

// operates with documents, but contains some additional content
internal class ContentDocumentCommand(ComponentName document, ComponentName collection, string[] content, CommandType type, string strCmd)
    : DocumentCommand(document, collection, type, strCmd), IContentCommand
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