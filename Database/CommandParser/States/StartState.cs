namespace DatabaseNS.CommandParserNS.States;

using DatabaseNS.Tokenization;
using DatabaseNS.ResultNS.Handlers;
using DatabaseNS.CommandParserNS.Commands;

internal class StartState : State {
    public StartState() : base() {}
    public override State NextState(Token token) {
        if (token.IsLast) {
            throw Handlers.Error.ThrowCommandParserCommandEmpty();
        }

        switch (token.Word) {
            case "get":
                builder.Type = CommandType.Find;
                return new DocumentState(this, Token.From);
            case "create":
                builder.Type = CommandType.Create;
                return new CollectionState(this);
            case "add":
                builder.Type = CommandType.Create;
                return new AddState(this);
            case "remove":
                builder.Type = CommandType.Delete;
                return new RemoveState(this);
            case "find":
                builder.Type = CommandType.Find;
                return new FindState(this);
            case "load":
                builder.Type = CommandType.Load;
                return new OneContentToCollectionState(this, Token.As);
            case "treshhold":
                builder.Type = CommandType.Treshhold;
                return new OneContentToCollectionState(this, Token.In);
            case "list":
                builder.Type = CommandType.List;
                return new ListState(this);
            case "save":
                builder.Type = CommandType.Save;
                return new EmptyState(this);
            default:
                throw Handlers.Error.ThrowCommandParseInvalidToken(token);
        }
    }
}