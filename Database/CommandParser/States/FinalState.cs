namespace DatabaseNS.CommandParserNS.States;

using DatabaseNS.ResultNS.Handlers;
using DatabaseNS.Tokenization;
using DatabaseNS.CommandParserNS.Commands;

// Last state of the state machine. Only syntactically correct commands can reach it
internal class FinalState : State {

    public FinalState(State state) : base(state) {}

    public override State NextState(Token token) {
        throw Handlers.Exception.ThrowCommandParseInvalidToken(token);
    }

    public Command GetCommand(string command) {
        return builder.Build(command);
    }
}