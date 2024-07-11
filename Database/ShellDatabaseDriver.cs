namespace DatabaseNS;

using DatabaseNS.CommandParserNS;
using DatabaseNS.Components;
using DatabaseNS.Components.Builders;

public class ShellDatabaseDriver : DatabaseDriver{

    private Database? _database;

    public override Result Execute(string stringCommand) {
        try {
            Command command = CommandParser.Parse(stringCommand);
            return processCommand(command);
        } catch (Exception e) when (e is InvalidOperationException || e is CommandParseException || e is DatabaseLoadException) {
            return new Result(e.Message);
        } 
    }

    private Result processCommand(Command command) {
        if (_database != null) {
            switch (command.Type) {
                case CommandType.CreateColletion:
                    if (command.CollectionName.HasValue)
                        return _database.CreateCollection(command.CollectionName.Value);
                    else
                        throw new InvalidOperationException(ErrorMessages.COMMAND_INVALID);
                case CommandType.DropCollection:
                    if (command.CollectionName.HasValue)
                        return _database.DropCollection(command.CollectionName.Value);
                    else
                        throw new InvalidOperationException(ErrorMessages.COMMAND_INVALID);
                case CommandType.GetDocument:
                    if (command.CollectionName.HasValue && command.DocumentName.HasValue) {
                        return _database.GetDocument(command.CollectionName.Value, command.DocumentName.Value);
                    } else {
                        throw new InvalidOperationException(ErrorMessages.COMMAND_INVALID);
                    }
                case CommandType.AddDocument:
                    if (command.CollectionName.HasValue && command.DocumentName.HasValue && command.Content.Length == 1) {
                        return _database.AddDocument(command.CollectionName.Value, command.DocumentName.Value, command.Content[0]);
                    } else {
                        throw new InvalidOperationException(ErrorMessages.COMMAND_INVALID);
                    }
                case CommandType.RemoveDocument:
                    if (command.CollectionName.HasValue && command.DocumentName.HasValue) {
                        return _database.RemoveDocument(command.CollectionName.Value, command.DocumentName.Value);
                    } else {
                        throw new InvalidOperationException(ErrorMessages.COMMAND_INVALID);
                    }
                case CommandType.SetTreshhold:
                    if (command.CollectionName.HasValue && command.Content.Length == 1) {
                        double treshhold;
                        if (Double.TryParse(command.Content[0], out treshhold)) {
                            return _database.SetTreshhold(command.CollectionName.Value, treshhold);
                        } else {
                            throw new InvalidOperationException(ErrorMessages.TRESHHOLD_INVALID_VALUE);
                        }
                    } else
                        throw new InvalidOperationException(ErrorMessages.COMMAND_INVALID);
                case CommandType.Exit:
                    _database.ShutDown();
                    return new Result("Database shutted down.", Environment.Exit);
                case CommandType.ShutDown:
                    _database.ShutDown();
                    _database = null;
                    return new Result("Database shutted down.");
                case CommandType.Find:
                    if (command.CollectionName.HasValue)
                        return _database.Find(command.CollectionName.Value, command.Content);
                    else
                        throw new InvalidOperationException(ErrorMessages.COMMAND_INVALID);
                default:
                    throw new InvalidOperationException(ErrorMessages.COMMAND_INVALID);
            }
        } else {
            if (command.Type == CommandType.Start) {
                var builder = new DatabaseBuilder();

                if (command.Content != null && command.Content.Length == 1)
                    builder.Path = new ComponentPath(command.Content[0]);
                else
                    builder.Path = new ComponentPath("Data");

                _database = Database.BuildFrom(builder);
                return new Result("Database started.");
            } else if (command.Type == CommandType.Exit) {
                return new Result("Database exited.", Environment.Exit);
            }
            else
                return new Result("Database need to be started.");
        }
    }
}