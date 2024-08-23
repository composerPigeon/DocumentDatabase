using DatabaseNS.Tokenization;

namespace DatabaseNS.CommandParserNS.States;

// State which expects to read one word to its internall buffer and then find some specified keyword (in this context split token)
internal class OneContentToCollectionState : SplitTokenBufferState {

    public OneContentToCollectionState(State state, Token splitToken) : base(state, BufferType.One, splitToken) {}

    public override State NextState(Token token) {
        if (token == splitToken) {
            builder.Content = getContentFromBuffer();
            return new CollectionState(this);
        } else {
            addTokenToBuffer(token);
            return this;
        }
    }

}