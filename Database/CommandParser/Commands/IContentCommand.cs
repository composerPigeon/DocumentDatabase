using DatabaseNS.FileSystem;

namespace DatabaseNS.CommandParserNS.Commands;

// API which ensures that Content command will have appropriate functions for handling content
internal interface IContentCommand {
    protected string[] Content { get; }

    public int ContentLength => Content.Length;

    public bool TryGetDouble(int pos, out double value);
        
    
    public bool TryGetString(int pos, out string value);
}