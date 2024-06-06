namespace DatabaseNS;

using Database_DbComponents;

public abstract class DatabaseDriver {
    public string Path { get; }
    public DatabaseDriver(string directoryPath) {
        Path = directoryPath;
    }
    public abstract Result Execute(string command);    
}