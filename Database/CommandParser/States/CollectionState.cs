namespace DatabaseNS.CommandParserNS.States;

using DatabaseNS.Tokenization;
using DatabaseNS.FileSystem;
using DatabaseNS.ResultNS.Handlers;

internal class CollectionState : State {
    public CollectionState(State state) : base(state) {}

    public override State NextState(Token token) {
        if (token.IsLast)
            throw Handlers.Error.ThrowCommandParseInvalidToken(token);
        if (token.Word != null)
            CollectionName = token.Word.ToName();
        return new FinalState(this);
    }
}