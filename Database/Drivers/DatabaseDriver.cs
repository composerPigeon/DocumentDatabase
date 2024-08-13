namespace DatabaseNS.Drivers;

using DatabaseNS.Components;
using DatabaseNS.ResultNS;
using DatabaseNS.ResultNS.Exceptions;
using DatabaseNS.CommandParserNS.States;
using DatabaseNS.CommandParserNS.Commands;
using DatabaseNS.FileSystem;
using DatabaseNS.ResultNS.Handlers;

public class DatabaseDriver {
    internal Database Database { get; }

    private DatabaseDriver(Database database) {
        Database = database;
    }

    private static DatabaseDriver _instance = initDriver();
    public static DatabaseDriver Instance {
        get { return _instance; }
    }

    public Result Execute(string input) {
        try {
            Command command = CommandParser.Parse(input);
            return ProcessCommand(command);
        } catch (ResultException e)  {
            return e.Result;
        } catch (Exception e) {
            return //TODO cover unexpected exceptions;
        }
    }

    private Result processCreateCmd(Command command) {
        if (command is CollectionCommand collectionCommand) {
            return Database.CreateCollection(collectionCommand.Collection);
        } else if (command is ContentDocumentCommand contentDocumentCommand) {
            string content;
            if (contentDocumentCommand.TryGetString(0, out content)) {
                return Database.AddDocument(
                    contentDocumentCommand.Collection,
                    contentDocumentCommand.Document,
                    content
                );
            }
        }
        return Handlers.Error.HandleCommandInvalid(command.Value);
    }

    private Result processDeleteCmd(Command command) {
        if (command is CollectionCommand collectionCommand) {
            return Database.RemoveCollection(collectionCommand.Collection);
        } else if (command is DocumentCommand documentCommand) {
            return Database.RemoveDocument(documentCommand.Collection, documentCommand.Document);
        } else if {
            //Bulk remove
        }
        return Handlers.Error.HandleCommandInvalid(command.Value);
    }

    private Result processFindCmd(Command command) {
        if (command is ContentCollectionCommand contentCollectionCommand) {
           return Database.Find(contentCollectionCommand.Collection, contentCollectionCommand.Content); 
        } else if (command is DocumentCommand documentCommand) {
            return Database.GetDocument(documentCommand.Collection, documentCommand.Document);
        }
        return Handlers.Error.HandleCommandInvalid(command.Value);
    }

    private Result processLoadCmd(Command command) {
        if (command is ContentCollectionCommand contentCollectionCommand) {
            // Bulk load of documents
        }
        return Handlers.Error.HandleCommandInvalid(command.Value);
    }

    private Result processTreshholdCmd(Command command) {
        if (command is ContentCollectionCommand contentCollectionCmd) {
            double value;
            if (contentCollectionCmd.TryGetDouble(0, out value))
                return Database.SetTreshhold(contentCollectionCmd.Collection, value);
        }
        return Handlers.Error.HandleCommandInvalid(command.Value);
    }

    private Result processListCmd(Command command) {
        if (command is CollectionCommand) {
            //LIst documents in database
        } else {
            //List collections
        }
    }

    internal Result ProcessCommand(Command command) {
        switch (command.Type) {
            case CommandType.Create:
                return processCreateCmd(command);
            case CommandType.Delete:
                return processDeleteCmd(command);
            case CommandType.Find:
                return processFindCmd(command);
            case CommandType.Load:
                return processLoadCmd(command);
            case CommandType.Treshhold:
                return processTreshholdCmd(command);
            case CommandType.List:
                return processListCmd(command);
            default:
                return Handlers.Error.HandleCommandInvalid(command.Value);
        }
    }

    private static DatabaseDriver initDriver() {
        var database = FileSystemAccessHandler.LoadDatabase();
        return new DatabaseDriver(database);
    }
}