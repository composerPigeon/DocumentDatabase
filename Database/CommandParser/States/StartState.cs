namespace DatabaseNS.CommandParserNS.States;

using DatabaseNS.Tokenization;
using DatabaseNS.ResultNS.Exceptions;
using DatabaseNS.ResultNS.Handlers;

internal class StartState : State {
    public StartState() : base(new List<string>()) {}
    public override State NextState(Token token) {
        if (token.IsLast) {
            throw Handlers.Error.ThrowCommandParseShort();
        }

        switch (token.Word) {
            case "start":
                Type = CommandType.Start;
                return new FinalState(this);
            case "exit":
                Type = CommandType.Exit;
                return new FinalState(this);
            case "shutdown":
                Type = CommandType.ShutDown;
                return new FinalState(this);
            case "get":
                Type = CommandType.GetDocument;
                return new DocumentState(this);
            case "create":
                Type = CommandType.CreateColletion;
                return new CollectionState(this);
            case "drop":
                Type = CommandType.DropCollection;
                return new CollectionState(this);
            case "add":
                Type = CommandType.AddDocument;
                return new ContentState(this);
            case "remove":
                Type = CommandType.RemoveDocument;
                return new DocumentState(this);
            case "find":
                Type = CommandType.Find;
                return new ContentState(this);
            case "load":
                Type = CommandType.Load;
                return new ContentState(this);
            case "treshhold":
                Type = CommandType.SetTreshhold;
                return new ContentState(this);
            default:
                throw Handlers.Error.ThrowCommandParseInvalidToken(token);
        }
    }
}