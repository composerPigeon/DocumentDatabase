using DatabaseNS.Tokenization;

namespace DatabaseNS.CommandParserNS.States;

internal abstract class SplitTokenBufferState : BufferState {
    protected Token splitToken;

    public SplitTokenBufferState(State state, BufferType bufferType, Token splitToken) : base(state, bufferType) {
        this.splitToken = splitToken;
    }
}