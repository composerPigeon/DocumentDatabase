namespace DatabaseNS.Drivers;

using DatabaseNS.CommandParserNS;
using DatabaseNS.Components;
using DatabaseNS.Messages;

internal class ShellDatabaseDriver : DatabaseDriver {
    internal ShellDatabaseDriver() {}
    internal override DatabaseResult ProcessCommand(Command command) {
        switch (command.Type) {
            case CommandType.CreateColletion:
                if (command.CollectionName.HasValue)
                    return Database.CreateCollection(command.CollectionName.Value);
                break;
            case CommandType.DropCollection:
                if (command.CollectionName.HasValue)
                    return Database.DropCollection(command.CollectionName.Value);
                break;
            case CommandType.GetDocument:
                if (command.CollectionName.HasValue && command.DocumentName.HasValue) {
                    return Database.GetDocument(command.CollectionName.Value, command.DocumentName.Value);
                } break;
            case CommandType.AddDocument:
                if (command.CollectionName.HasValue && command.DocumentName.HasValue && command.Content.Length == 1) {
                    return Database.AddDocument(command.CollectionName.Value, command.DocumentName.Value, command.Content[0]);
                } break;
            case CommandType.RemoveDocument:
                if (command.CollectionName.HasValue && command.DocumentName.HasValue) {
                    return Database.RemoveDocument(command.CollectionName.Value, command.DocumentName.Value);
                } break;
            case CommandType.SetTreshhold:
                if (command.CollectionName.HasValue && command.Content.Length == 1) {
                    double treshhold;
                    if (Double.TryParse(command.Content[0], out treshhold)) {
                        return Database.SetTreshhold(command.CollectionName.Value, treshhold);
                    } else {
                        return DatabaseResult.BadRequest(ErrorMessages.TreshholdInvalidValue(command.Content[0]));
                    }
                } break;
            case CommandType.Find:
                if (command.CollectionName.HasValue)
                    return Database.Find(command.CollectionName.Value, command.Content);
                break;
            case CommandType.Load:
                if (command.CollectionName.HasValue && command.Content.Length == 1)
                    return new DatabaseResult();
                break;
        }
        return DatabaseResult.BadRequest(ErrorMessages.CommandInvalid(command));
    }
}