using DatabaseNS.Tokenization;

namespace DatabaseNS.CommandParserNS.States;


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