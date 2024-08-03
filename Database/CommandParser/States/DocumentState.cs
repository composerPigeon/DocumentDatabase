namespace DatabaseNS.CommandParserNS.States;

using DatabaseNS.Tokenization;
using DatabaseNS.Components.FileSystem;
using DatabaseNS.ResultNS.Handlers;

internal class DocumentState : State {
    public DocumentState(State state) : base(state) {}

    private State nextStateWithOneDocumentExpected(Token token, string keyWord) {
        if (token.Word != null)
            if (token.Word == keyWord) {
                if (DocumentName != null)
                    return new CollectionState(this);
                else
                    throw Handlers.Error.ThrowCommandParseInvalidToken(token);
            } else {
                DocumentName = token.Word.ToName();
                return this;
            }
        else
            throw Handlers.Error.ThrowCommandParseShort();
    }

    public override State NextState(Token token) {
        switch(Type) {
            case CommandType.GetDocument:
                return nextStateWithOneDocumentExpected(token, "from");
            case CommandType.AddDocument:
                return nextStateWithOneDocumentExpected(token, "to");
            case CommandType.RemoveDocument:
                return nextStateWithOneDocumentExpected(token, "from");
            default:
                throw Handlers.Error.ThrowCommandInvalid();
        }
    }
}