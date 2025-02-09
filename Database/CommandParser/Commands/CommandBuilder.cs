namespace DatabaseNS.CommandParserNS.Commands;

using DatabaseNS.Components.Values;
using DatabaseNS.ResultNS.Handlers;

// Checks setted properties and based on them create correct instance of command
internal class CommandBuilder {
    public CommandType? Type { set; private get;}
    public ComponentName? Collection { set; private get;}
    public ComponentName? Document { set; private get;}
    public List<string>? Content { set; private get;}

    private bool TryBuildCollectionCmd(out Command command, string strCmd)
    {
        command = Command.Empty;
        if (!Collection.HasValue || !Type.HasValue)
            return false;

        command = new CollectionCommand(Collection.Value, Type.Value, strCmd);
        return true;
    }

    private bool TryBuildContentCollectionCmd(out Command command, string strCmd)
    {
        command = Command.Empty;
        if (!Collection.HasValue || !Type.HasValue || Content == null || Content.Count == 0)
            return false;
        
        command = new ContentCollectionCommand(Collection.Value, Content.ToArray(), Type.Value, strCmd);
        return true;
    }

    private bool TryBuildContentDocumentCmd(out Command command, string strCmd)
    {
        command = Command.Empty;
        if (!Collection.HasValue || !Type.HasValue || !Document.HasValue || Content == null || Content.Count == 0)
            return false;
        
        command = new ContentDocumentCommand(Document.Value, Collection.Value, Content.ToArray(), Type.Value, strCmd);
        return true;
    }

    private bool TryBuildDocumentCmd(out Command command, string strCmd)
    {
        command = Command.Empty;
        if (!Collection.HasValue || !Type.HasValue || !Document.HasValue)
            return false;
        command = new DocumentCommand(Document.Value, Collection.Value, Type.Value, strCmd);
        return true;
    }

    private bool TryBuildCmd(out Command command, string strCmd)
    {
        command = Command.Empty;
        if (!Type.HasValue)
            return false;
       
        command = new Command(Type.Value, strCmd);
        return true;
    }

    public Command Build(string stringCommand)
    {
        Command command;
        if (TryBuildContentDocumentCmd(out command, stringCommand))
            return command;
        if (TryBuildContentCollectionCmd(out command, stringCommand))
            return command;
        if (TryBuildDocumentCmd(out command, stringCommand))
            return command;
        if (TryBuildCollectionCmd(out command, stringCommand))
            return command;
        if (TryBuildCmd(out command, stringCommand))
            return command;
        
        throw Handlers.Exception.ThrowCommandInvalid(stringCommand);
    }
}