namespace DatabaseNS.CommandParserNS.States;

using DatabaseNS.ResultNS.Handlers;
using DatabaseNS.Tokenization;

internal class ContentState : State {
    public ContentState(State state) : base(state) {}

    private State nextStateWithOneContent(Token token, string keyWord, NextStateType nextState) {
        if (token.Word != null) {
            if (token.Word == keyWord) {
                if (Content.Count == 1) {
                    switch (nextState) {
                        case NextStateType.Document:
                            return new DocumentState(this);
                        case NextStateType.Collection:
                            return new CollectionState(this);
                        default:
                            return this;
                    }
                }
                else
                    throw Handlers.Error.ThrowCommandParseInvalidToken(token);
            } else {
                Content.Add(token.Word);
                if (Content.Count > 1)
                    throw Handlers.Error.ThrowCommandParseInvalidToken(token);
                return this;
                
            }
        } else if (token.IsLast)
            throw Handlers.Error.ThrowCommandParseInvalidToken(token);
        else
            throw Handlers.Error.ThrowCommandParseShort();
    }

    private State nextStateWithFind(Token token) {
        if (token.Word != null)
            if (token.Word == "in") {
                return new CollectionState(this);
            } else {
                Content.Add(token.Word);
                return this;
            }
        else
            throw Handlers.Error.ThrowCommandParseShort();
    }

    public override State NextState(Token token) {
        switch(Type) {
            case CommandType.AddDocument:
                return nextStateWithOneContent(token, "as", NextStateType.Document);
            case CommandType.Find:
                return nextStateWithFind(token);
            case CommandType.Load:
                return nextStateWithOneContent(token, "as", NextStateType.Collection);
            case CommandType.SetTreshhold:
                return nextStateWithOneContent(token, "for", NextStateType.Collection);
            default:
                throw Handlers.Error.ThrowCommandParseInvalidState();
        }
    }

    private enum NextStateType {
        Document,
        Collection
    }
}