using DatabaseNS.Tokenization;

namespace DatabaseNS.CommandParserNS.States;

internal class ListState : BufferState {
    public ListState(State state) : base(state, BufferType.One) {}

    public override State NextState(Token token) {
        if (token == Token.Last && bufferCount == 0) {
            return new FinalState(this);
        } else if (token == Token.Last && bufferCount == 1) {
            builder.Collection = getNameFromBuffer();
            return new CollectionState(this);
        } else {
            addTokenToBuffer(token);
            return this;
        }
    }
}