using DatabaseNS.FileSystem;

namespace DatabaseNS.CommandParserNS.Commands;

internal interface IContentCommand {
    public string[] Content { get; }

    public int ContentLength {
        get { return Content.Length; }
    }

    public bool TryGetDouble(int pos, out double value);
        
    
    public bool TryGetString(int pos, out string value);
}