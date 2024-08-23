using DatabaseNS.Tokenization;

namespace DatabaseNS.CommandParserNS.States;

// State which expects to find collectionName and then EOF to change to the final state
internal class CollectionState : BufferState {
    public CollectionState(State state) : base(state, BufferType.One) {}
    public override State NextState(Token token) {
        if (token.IsLast) {
            builder.Collection = getNameFromBuffer();
            return new FinalState(this);
        } else {
            addTokenToBuffer(token);
            return this;
        }
    }
}