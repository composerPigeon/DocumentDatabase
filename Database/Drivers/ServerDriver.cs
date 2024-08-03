namespace DatabaseNS.Drivers;

using DatabaseNS.CommandParserNS;
using DatabaseNS.Components;
using DatabaseNS.ResultNS;

public class ServerDatabaseDriver : DatabaseDriver {
    internal ServerDatabaseDriver() {}
    internal override Result ProcessCommand(Command command) {
        switch (command.Type) {
            case CommandType.CreateColletion:
                if (command.CollectionName.HasValue)
                    return Database.CreateCollection(command.CollectionName.Value);
                else
                    throw new InvalidOperationException(Messages.COMMAND_INVALID);
            case CommandType.DropCollection:
                if (command.CollectionName.HasValue)
                    return Database.DropCollection(command.CollectionName.Value);
                else
                    throw new InvalidOperationException(Messages.COMMAND_INVALID);
            case CommandType.GetDocument:
                if (command.CollectionName.HasValue && command.DocumentName.HasValue) {
                    return Database.GetDocument(command.CollectionName.Value, command.DocumentName.Value);
                } else {
                    throw new InvalidOperationException(Messages.COMMAND_INVALID);
                }
            case CommandType.AddDocument:
                if (command.CollectionName.HasValue && command.DocumentName.HasValue && command.Content.Length == 1) {
                    return Database.AddDocument(command.CollectionName.Value, command.DocumentName.Value, command.Content[0]);
                } else {
                    throw new InvalidOperationException(Messages.COMMAND_INVALID);
                }
            case CommandType.RemoveDocument:
                if (command.CollectionName.HasValue && command.DocumentName.HasValue) {
                    return Database.RemoveDocument(command.CollectionName.Value, command.DocumentName.Value);
                } else {
                    throw new InvalidOperationException(Messages.COMMAND_INVALID);
                }
            case CommandType.SetTreshhold:
                if (command.CollectionName.HasValue && command.Content.Length == 1) {
                    double treshhold;
                    if (Double.TryParse(command.Content[0], out treshhold)) {
                        return Database.SetTreshhold(command.CollectionName.Value, treshhold);
                    } else {
                        throw new InvalidOperationException(Messages.TRESHHOLD_INVALID_VALUE);
                    }
                } else
                    throw new InvalidOperationException(Messages.COMMAND_INVALID);
            case CommandType.Find:
                if (command.CollectionName.HasValue)
                    return Database.Find(command.CollectionName.Value, command.Content);
                else
                    throw new InvalidOperationException(Messages.COMMAND_INVALID);
            default:
                throw new InvalidOperationException(Messages.COMMAND_INVALID);
        }
    }
}