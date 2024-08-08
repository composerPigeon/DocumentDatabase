using System.Reflection.Metadata;
using DatabaseNS.FileSystem;
using DatabaseNS.ResultNS.Handlers;

namespace DatabaseNS.CommandParserNS.Commands;

internal class CommandBuilder {
    public CommandType? Type { set; private get;}
    public ComponentName? Collection { set; private get;}
    public ComponentName? Document { set; private get;}
    public List<string>? Content { set; private get;}

    private bool tryBuildCollectionCmd(out Command command) {
        command = Command.Empty;
        if (Collection.HasValue && Type.HasValue) {
            command = new CollectionCommand(Collection.Value, Type.Value);
            return true;
        }
        return false;
    }

    private bool tryBuildContentCollectionCmd(out Command command) {
        command = Command.Empty;
        if (Collection.HasValue && Type.HasValue && Content != null && Content.Count > 0) {
            command = new ContentCollectionCommand(Collection.Value, Content.ToArray(), Type.Value);
            return true;
        }
        return false;
    }

    private bool tryBuildContentDocumentCmd(out Command command) {
        command = Command.Empty;
        if (Collection.HasValue && Type.HasValue && Document.HasValue && Content != null && Content.Count > 0) {
            command = new ContentDocumentCommand(Document.Value, Collection.Value, Content.ToArray(), Type.Value);
            return true;
        }
        return false;
    }

    private bool tryBuildDocumentCmd(out Command command) {
        command = Command.Empty;
        if (Collection.HasValue && Type.HasValue && Document.HasValue) {
            command = new DocumentCommand(Document.Value, Collection.Value, Type.Value);
            return true;
        }
        return false;
    }

    private bool tryBuildCmd(out Command command) {
        command = Command.Empty;
        if (Type.HasValue) {
            command = new Command(Type.Value);
            return true;
        }
        return false;
    }

    public Command Build(string stringCommand) {
        Command command;
        if (!tryBuildContentDocumentCmd(out command)) {
            if (!tryBuildContentCollectionCmd(out command)) {
                if (!tryBuildDocumentCmd(out command)) {
                    if (!tryBuildCollectionCmd(out command)) {
                        if (!tryBuildCmd(out command))
                            throw Handlers.Error.ThrowCommandInvalid(stringCommand);
                    }
                }
            }
        }
        return command;
    }
}