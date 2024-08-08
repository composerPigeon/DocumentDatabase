using DatabaseNS.ResultNS.Handlers;
using DatabaseNS.Tokenization;

namespace DatabaseNS.CommandParserNS.States;

internal class EmptyState : State {
    public EmptyState(State state) : base(state) {}

    public override State NextState(Token token) {
        if (token.IsLast) {
            return new FinalState(this);
        }
        throw Handlers.Error.ThrowCommandParseInvalidToken(token);
    }
}