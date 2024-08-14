namespace DatabaseNS.Drivers;

using DatabaseNS.Components;
using DatabaseNS.Components.Values;
using DatabaseNS.ResultNS;
using DatabaseNS.ResultNS.Exceptions;
using DatabaseNS.CommandParserNS.States;
using DatabaseNS.CommandParserNS.Commands;
using DatabaseNS.FileSystem;
using DatabaseNS.ResultNS.Handlers;

public class DatabaseDriver {
    internal Database Database { get; }
    internal DriverType Type { get; }

    private DatabaseDriver(Database database, DriverType type) {
        Database = database;
        Type = type;
    }

    public Result Execute(string input) {
        try {
            Command command = CommandParser.Parse(input);
            return ProcessCommand(command);
        } catch (ResultException e)  {
            return e.Result;
        } catch (Exception e) {
            return Handlers.Error.HandleUnexpectedException(e);
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
            ComponentPath path = "".AsPath();
            if (contentCollectionCommand.TryGetPath(0, out path)) {
                if (Type == DriverType.Server) {
                    return Handlers.Error.HandleCommandNotSupported(command.Value);
                }
                return Database.LoadDocuments(contentCollectionCommand.Collection, path);
            }
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
        if (command is CollectionCommand collectionCommand) {
            return Database.ListDocuments(collectionCommand.Collection);
        } else {
            return Database.ListCollections();
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

    public static DatabaseDriver InitializeDriver(DriverType type) {
        var database = FileSystemAccessHandler.LoadDatabase();
        return new DatabaseDriver(database, type);
    }
}

public enum DriverType {
    Server,
    Shell
}