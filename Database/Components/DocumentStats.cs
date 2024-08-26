namespace DatabaseNS.Components;

using System.Text.Json.Serialization;
using DatabaseNS.Components.Builders;
using DatabaseNS.Components.Values;
using DatabaseNS.DocumentParserNS;

internal class DocumentStats : DatabaseComponent, IDatabaseComponentBuilderCreatable<DocumentStats, DocumentStatsBuilder> {

    // Word and TermFreq
    public Dictionary<string, double> WordsTF { get;}

    [JsonConstructor]
    private DocumentStats(ComponentName name, ComponentPath path, Dictionary<string, double> wordsTF) : base(name, path) {
        WordsTF = wordsTF;
    }

    // create based on content
    public static DocumentStats Create(ComponentName name, ComponentPath path, string documentContent) {
        WordCounter counter = DocumentParser.Parse(documentContent);
        DocumentStatsBuilder builder = CreateBuilder();
        builder.WordsTF = counter.Calculate();
        builder.Path = path;
        builder.Name = name;
        return builder.Build();
    }

    // create based on filePath which is loaded as document content
    public static DocumentStats Create(ComponentName name, ComponentPath path, ComponentPath filePath) {
        WordCounter counter = DocumentParser.Parse(filePath);
        DocumentStatsBuilder builder = CreateBuilder();
        builder.WordsTF = counter.Calculate();
        builder.Path = path;
        builder.Name = name;
        return builder.Build();
    }

    public static DocumentStatsBuilder CreateBuilder() {
        return new DocumentStatsBuilder((name, path, wordsTF) => new DocumentStats(name, path, wordsTF));
    }
}