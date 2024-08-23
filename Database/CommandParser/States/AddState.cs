using DatabaseNS.Tokenization;

namespace DatabaseNS.CommandParserNS.States;

// State when first word in the stream was "add"
internal class AddState : BufferState {
    public AddState(State state) : base(state, BufferType.One) {}
    public override State NextState(Token token) {
        if (token == Token.As) {
            builder.Content = getContentFromBuffer();
            return new DocumentState(this, Token.To);
        } else {
            addTokenToBuffer(token);
            return this;
        }
    }
}