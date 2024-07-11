namespace DatabaseNS.Components.Builders;

internal class DatabaseBuilder : DatabaseComponentBuilder {
    public Dictionary<ComponentName, Collection>? Collections { get; set; }
}