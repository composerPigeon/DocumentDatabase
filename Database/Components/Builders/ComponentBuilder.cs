namespace DatabaseNS.Components.Builders;

using DatabaseNS.FileSystem;

internal abstract class DatabaseComponentBuilder<TComponent> {
    public ComponentName? Name { get; set; }
    public ComponentPath? Path { get; set; }
    public abstract TComponent Build();
}