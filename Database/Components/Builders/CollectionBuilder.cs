namespace DatabaseNS.Components.Builders;

using DatabaseNS.Components.IndexNS;

internal class CollectionBuilder : DatabaseComponentBuilder {
    public Index? Index { get; set; }
    public Dictionary<ComponentName, Document>? Documents { get; set; }
}