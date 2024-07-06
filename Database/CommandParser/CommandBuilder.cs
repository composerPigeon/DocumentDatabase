namespace DatabaseNS.CommandParserNS;

using DatabaseNS.Components;
using DatabaseNS;

internal class CommandBuilder {
    
    private ComponentName? _collectionName;
    public CommandType? Type {private get; set;}

    private ComponentName? _documentName;
    private List<string> _content;

    public CommandBuilder() {
        _content = new List<string>();
    }

    public void AddKeyWord(string? content) {
        if (string.IsNullOrEmpty(content))
            return;
        this._content.Add(content);
    }

    public void SetCollection(string name) {
        if (string.IsNullOrEmpty(name))
            return;
        _collectionName = new ComponentName(name);   
    }

    public void SetDocument(string name) {
        if (string.IsNullOrEmpty(name))
            return;
        _documentName = new ComponentName(name);
    }

    public Command Build() {
        if (!_collectionName.HasValue || !Type.HasValue)
            throw new ArgumentNullException(ErrorMessages.COMMAND_INVALID);
        
        return new Command() {
            CollectionName = _collectionName.Value,
            Type = Type.Value,
            DocumentName = _documentName,
            Content = _content.ToArray()
        };
        
    }
}