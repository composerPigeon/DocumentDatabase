namespace DatabaseNS.CommandParserNS.States;

using DatabaseNS.FileSystem;
using DatabaseNS.Tokenization;

internal abstract class State {
    public ComponentName? CollectionName {get; protected set;}
    public CommandType? Type {get; protected set;}

    public ComponentName? DocumentName {get; protected set;}
    public List<string> Content {get; init;}

    protected State(List<string> content) {
        Content = content;
    }

    protected State(State state) {
        CollectionName = state.CollectionName;
        Type = state.Type;
        DocumentName = state.DocumentName;
        Content = state.Content;
    }

    public abstract State NextState(Token token);
}