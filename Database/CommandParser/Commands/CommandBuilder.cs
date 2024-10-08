namespace DatabaseNS.CommandParserNS.Commands;

using DatabaseNS.Components.Values;
using DatabaseNS.ResultNS.Handlers;

// Checks setted properties and based on them create correct instance of command
internal class CommandBuilder {
    public CommandType? Type { set; private get;}
    public ComponentName? Collection { set; private get;}
    public ComponentName? Document { set; private get;}
    public List<string>? Content { set; private get;}

    private bool tryBuildCollectionCmd(out Command command, string strCmd) {
        command = Command.Empty;
        if (Collection.HasValue && Type.HasValue) {
            command = new CollectionCommand(Collection.Value, Type.Value, strCmd);
            return true;
        }
        return false;
    }

    private bool tryBuildContentCollectionCmd(out Command command, string strCmd) {
        command = Command.Empty;
        if (Collection.HasValue && Type.HasValue && Content != null && Content.Count > 0) {
            command = new ContentCollectionCommand(Collection.Value, Content.ToArray(), Type.Value, strCmd);
            return true;
        }
        return false;
    }

    private bool tryBuildContentDocumentCmd(out Command command, string strCmd) {
        command = Command.Empty;
        if (Collection.HasValue && Type.HasValue && Document.HasValue && Content != null && Content.Count > 0) {
            command = new ContentDocumentCommand(Document.Value, Collection.Value, Content.ToArray(), Type.Value, strCmd);
            return true;
        }
        return false;
    }

    private bool tryBuildDocumentCmd(out Command command, string strCmd) {
        command = Command.Empty;
        if (Collection.HasValue && Type.HasValue && Document.HasValue) {
            command = new DocumentCommand(Document.Value, Collection.Value, Type.Value, strCmd);
            return true;
        }
        return false;
    }

    private bool tryBuildCmd(out Command command, string strCmd) {
        command = Command.Empty;
        if (Type.HasValue) {
            command = new Command(Type.Value, strCmd);
            return true;
        }
        return false;
    }

    public Command Build(string stringCommand) {
        Command command;
        if (!tryBuildContentDocumentCmd(out command, stringCommand)) {
            if (!tryBuildContentCollectionCmd(out command, stringCommand)) {
                if (!tryBuildDocumentCmd(out command, stringCommand)) {
                    if (!tryBuildCollectionCmd(out command, stringCommand)) {
                        if (!tryBuildCmd(out command, stringCommand))
                            throw Handlers.Exception.ThrowCommandInvalid(stringCommand);
                    }
                }
            }
        }
        return command;
    }
}