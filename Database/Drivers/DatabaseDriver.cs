namespace DatabaseNS.Drivers;

using DatabaseNS.Components;
using DatabaseNS.CommandParserNS;
using DatabaseNS.ResultNS;
using DatabaseNS.ResultNS.Exceptions;
using DatabaseNS.CommandParserNS.States;

public abstract class DatabaseDriver {
    private static DatabaseDriver _serverInstance = new ServerDatabaseDriver();
    public static DatabaseDriver Server {
        get {
            return _serverInstance;
        }
    }

    private static DatabaseDriver _shellInstance = new ShellDatabaseDriver();
    internal static DatabaseDriver Shell {
        get {
            return _shellInstance;
        }
    }

    private Database? _database;

    internal Database Database {
        get {
            if (_database == null) {
                _database = DatabaseLoader.Load("".AsPath());
            }
            return _database;
        }
    }
    public Result Execute(string input) {
        try {
            Command command = CommandParser.Parse(input);
            return ProcessCommand(command);
        } catch (ValueException e)  {
            return e.Value;
        } catch (Exception e) {
            return ;
        }
    }

    internal abstract Result ProcessCommand(Command command);
}