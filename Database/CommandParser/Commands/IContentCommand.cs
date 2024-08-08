using DatabaseNS.FileSystem;

namespace DatabaseNS.CommandParserNS.Commands;

internal interface IContentCommand {
    protected string[] Content { get; }

    public int ContentLength {
        get { return Content.Length; }
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
    public bool TryGetString(int pos, out string value) {
        value = "";
        if (Content.Length > pos && pos >= 0) {
            value = Content[pos];
            return true;
        }
        return false;
    }
}