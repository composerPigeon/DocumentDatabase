namespace DatabaseNS;

using DatabaseNS.Components;

public abstract class DatabaseDriver {
    public abstract Result Execute(string command);    
}