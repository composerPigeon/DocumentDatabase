namespace DatabaseNS.Components.Builders;

internal abstract class DatabaseComponentBuilder {
    public ComponentName? Name { get; set; }
    public ComponentPath? Path { get; set; }
}