namespace DatabaseNS.CommandParserNS.States;

using DatabaseNS.ResultNS.Handlers;
using DatabaseNS.Tokenization;

internal class FinalState : State {

    public FinalState(State state) : base(state) {}

    public override State NextState(Token token) {
        throw Handlers.Error.ThrowCommandParseInvalidState();
    }

    public Command GetCommand(string command) {
        if (!Type.HasValue) {
            throw Handlers.Error.ThrowCommandInvalid(command);
        }
        return new Command() {
            CollectionName = CollectionName,
            Type = Type.Value,
            DocumentName = DocumentName,
            Content = Content.ToArray()
        };
    }
}