namespace DatabaseNS.Components;

internal abstract class DatabaseComponent {
    public ComponentName Name { get; }
    public ComponentPath Path { get; }
    public DatabaseComponent(ComponentName name, ComponentPath path) {
        Name = name;
        Path = path;
    }
} 

internal abstract class DatabaseComponentFactory {}