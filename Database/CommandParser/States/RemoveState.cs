namespace DatabaseNS.CommandParserNS.States;

using DatabaseNS.Tokenization;

internal class RemoveState : BufferState {

    public RemoveState(State state) : base(state, BufferType.Any) {}
    public override State NextState(Token token) {
        if (token.IsLast && bufferCount == 1) {
            builder.Collection = getNameFromBuffer();
            return new FinalState(this);
        } else if (token == Token.From) {
            if (bufferCount == 1)
                builder.Document = getNameFromBuffer();
            else
                builder.Content = getContentFromBuffer();
            return new CollectionState(this);
        } else {
            addTokenToBuffer(token);
            return this;
        }
    }
}