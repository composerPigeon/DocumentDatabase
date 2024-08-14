namespace DatabaseNS.CommandParserNS.States;

using DatabaseNS.Tokenization;

internal class RemoveState : BufferState {

    public RemoveState(State state) : base(state, BufferType.One) {}
    public override State NextState(Token token) {
        if (token.IsLast) {
            builder.Collection = getNameFromBuffer();
            return new FinalState(this);
        } else if (token == Token.From) {
            builder.Document = getNameFromBuffer();
            return new CollectionState(this);
        } else {
            addTokenToBuffer(token);
            return this;
        }
    }
}