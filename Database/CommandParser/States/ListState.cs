using DatabaseNS.Tokenization;

namespace DatabaseNS.CommandParserNS.States;

// State when first word in the stream was "find"
internal class ListState : BufferState {
    public ListState(State state) : base(state, BufferType.One) {}

    public override State NextState(Token token) {
        if (token == Token.Last && bufferCount == 0) {
            return new FinalState(this);
        } else if (token == Token.Last && bufferCount == 1) {
            builder.Collection = getNameFromBuffer();
            return new FinalState(this);
        } else {
            addTokenToBuffer(token);
            return this;
        }
    }
}