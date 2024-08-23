namespace DatabaseNS.Components;

using DatabaseNS.Components.Values;

internal abstract class DatabaseComponent {
    public ComponentName Name { get; }
    public ComponentPath Path { get; }
    protected DatabaseComponent(ComponentName name, ComponentPath path) {
        Name = name;
        Path = path;
    }
}

// Used for specifying ComponentType in messages
internal enum ComponentType {
    Database,
    Collection,
    Index,
    Document,
    DocumentStats
}