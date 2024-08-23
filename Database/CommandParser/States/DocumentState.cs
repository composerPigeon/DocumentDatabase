using DatabaseNS.Tokenization;

namespace DatabaseNS.CommandParserNS.States;

// State which expects to find documentName and then some key word to change the state
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