namespace DatabaseNS.Components;

using DatabaseNS.Components.Builders;
using DatabaseNS.Components.Values;

internal interface IDatabaseComponentBuilderCreatable<TSelf, TBuilder>
    where TBuilder : DatabaseComponentBuilder<TSelf>
    where TSelf: DatabaseComponent
{
    abstract static TBuilder CreateBuilder();
}

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