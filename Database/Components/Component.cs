namespace DatabaseNS.Components;

using DatabaseNS.Components.Builders;

public class DatabaseCreateException : Exception {
    public DatabaseCreateException(string message) : base(message) {}
}

internal abstract class DatabaseComponent {
    public ComponentName Name { get; }
    public ComponentPath Path { get; }
    protected DatabaseComponent(ComponentName name, ComponentPath path) {
        Name = name;
        Path = path;
    }
}

internal interface IComponentBuildable<TComponent, TBuilder>
    where TComponent : DatabaseComponent
    where TBuilder : DatabaseComponentBuilder
{
    public static abstract TComponent BuildFrom(TBuilder builder);
}

internal interface IComponentCreatable<TComponent> where TComponent : DatabaseComponent {
    public static abstract TComponent Create(ComponentName name, ComponentPath path);
}