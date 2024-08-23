using DatabaseNS.Tokenization;

namespace DatabaseNS.CommandParserNS.States;

// State when first word in the stream was "find"
internal class FindState : BufferState {
    public FindState(State state) : base(state, BufferType.Any) {}

    public override State NextState(Token token) {
        if (token == Token.In && bufferCount > 0) {
            builder.Content = getContentFromBuffer();
            return new CollectionState(this);
        } else {
            addTokenToBuffer(token);
            return this;
        }
    }
}