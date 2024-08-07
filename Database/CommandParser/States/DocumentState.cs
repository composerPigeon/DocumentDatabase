using DatabaseNS.Tokenization;

namespace DatabaseNS.CommandParserNS.States;

internal class DocumentState : SplitTokenBufferState {
    public DocumentState(State state, Token splitToken) : base(state, BufferType.One, splitToken) {}

    public override State NextState(Token token) {
        if (token == splitToken) {
            builder.Document = getNameFromBuffer();
            return new CollectionState(this);
        } else {
            addTokenToBuffer(token);
            return this;
        }
    }
}