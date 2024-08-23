namespace DatabaseNS.Components.Builders;

using DatabaseNS.Components.Values;

// All components uses builder pattern for their initializing, constraints of consistency are checked in build methods
internal abstract class DatabaseComponentBuilder<TComponent> {
    public ComponentName? Name { get; set; }
    public ComponentPath? Path { get; set; }
    public abstract TComponent Build();
}