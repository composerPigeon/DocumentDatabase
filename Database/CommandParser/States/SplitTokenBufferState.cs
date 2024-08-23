using DatabaseNS.Tokenization;

namespace DatabaseNS.CommandParserNS.States;

// State which expects to find some other keyword determines that the state should change
internal abstract class SplitTokenBufferState : BufferState {
    protected Token splitToken;

    public SplitTokenBufferState(State state, BufferType bufferType, Token splitToken) : base(state, bufferType) {
        this.splitToken = splitToken;
    }
}