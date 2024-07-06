namespace DatabaseNS.CommandParserNS;

using DatabaseNS;
using DatabaseNS.Tokenization;
using DatabaseNS.Components;

internal abstract class State {
    public ComponentName? CollectionName {get; protected set;}
    public CommandType? Type {get; protected set;}

    public ComponentName? DocumentName {get; protected set;}
    public List<string> Content {get; init;}

    protected State(List<string> content) {
        Content = content;
    }

    protected State(State state) {
        CollectionName = state.CollectionName;
        Type = state.Type;
        DocumentName = state.DocumentName;
        Content = state.Content;
    }

    public abstract State NextState(Token token);
}

internal class StartState : State {
    public StartState() : base(new List<string>()) {}
    public override State NextState(Token token) {
        if (token.IsLast) {
            throw new CommandParseException(ErrorMessages.COMMANDPARSE_EOF);
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
            default:
                throw new CommandParseException(ErrorMessages.COMMANDPARSE_INVALID_TOKEN);
        }
    }
}

internal class CollectionState : State {
    public CollectionState(State state) : base(state) {}

    public override State NextState(Token token) {
        if (token.IsLast)
            throw new CommandParseException(ErrorMessages.COMMANDPARSE_INVALID_TOKEN);
        if (token.Word != null)
            CollectionName = token.Word.ToName();
        return new FinalState(this);
    }
}

internal class DocumentState : State {
    public DocumentState(State state) : base(state) {}

    private State nextStateWithOneDocumentExpected(Token token, string keyWord) {
        if (token.Word != null)
            if (token.Word == keyWord) {
                if (DocumentName != null)
                    return new CollectionState(this);
                else
                    throw new CommandParseException(ErrorMessages.COMMANDPARSE_INVALID_TOKEN);
            } else {
                DocumentName = token.Word.ToName();
                return this;
            }
        else
            throw new CommandParseException(ErrorMessages.COMMANDPARSE_EOF);
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
                throw new CommandParseException(ErrorMessages.COMMAND_INVALID);
        }
    }
}

internal class ContentState : State {
    public ContentState(State state) : base(state) {}

    private State nextStateWithAddDocument(Token token) {
        if (token.Word != null)
            if (token.Word == "as") {
                if (Content.Count == 1)
                    return new DocumentState(this);
                else
                    throw new CommandParseException(ErrorMessages.COMMANDPARSE_INVALID_TOKEN);
            } else {
                Content.Add(token.Word);
                if (Content.Count > 1) {
                    throw new CommandParseException(ErrorMessages.COMMANDPARSE_INVALID_TOKEN);
                }
                return this;
            }
        else
            throw new CommandParseException(ErrorMessages.COMMANDPARSE_EOF);
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
            throw new CommandParseException(ErrorMessages.COMMANDPARSE_EOF);
    }

    public override State NextState(Token token) {
        switch(Type) {
            case CommandType.AddDocument:
                return nextStateWithAddDocument(token);
            case CommandType.Find:
                return nextStateWithFind(token);
            default:
                throw new CommandParseException(ErrorMessages.COMMAND_INVALID);
        }
    }
}

internal class FinalState : State {

    public FinalState(State state) : base(state) {}

    public override State NextState(Token token) {
        throw new CommandParseException(ErrorMessages.COMMANDPARSE_LONG);
    }

    public Command GetCommand() {
        if (!Type.HasValue) {
            throw new CommandParseException(ErrorMessages.COMMAND_INVALID);
        }
        return new Command() {
            CollectionName = CollectionName,
            Type = Type.Value,
            DocumentName = DocumentName,
            Content = Content.ToArray()
        };
    }
}