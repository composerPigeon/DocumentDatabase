namespace DatabaseNS.CommandParserNS.States;

using DatabaseNS.ResultNS.Handlers;
using DatabaseNS.Tokenization;
using DatabaseNS.CommandParserNS.Commands;

internal class FinalState : State {

    public FinalState(State state) : base(state) {}

    public override State NextState(Token token) {
        throw Handlers.Error.ThrowCommandParseInvalidState();
    }

    public Command GetCommand(string command) {
        return builder.Build(command);
    }
}