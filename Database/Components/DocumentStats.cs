namespace DatabaseNS.Components;

using System.Text.Json;
using System.Text.Json.Serialization;
using DatabaseNS.Components.Builders;
using DatabaseNS.FileSystem;
using DatabaseNS.DocumentParserNS;

internal class DocumentStats : DatabaseComponent {

    // Word and TermFreq
    public Dictionary<string, double> WordsTF { get;}

    [JsonConstructor]
    private DocumentStats(ComponentName name, ComponentPath path, Dictionary<string, double> wordsTF) : base(name, path) {
        WordsTF = wordsTF;
    }

    public void Save(JsonSerializerOptions options) {
        string content = JsonSerializer.Serialize(this, options);
        Path.AsExecutable().Write(content);
    }

    public void Remove() {
        Path.AsExecutable().Remove();
    }

    public static DocumentStats ReadDocument(ComponentName name, ComponentPath path, string content) {
        WordCounter counter = DocumentParser.Parse(content);
        DocumentStatsBuilder builder = DocumentStats.CreateBuilder();
        builder.WordsTF = counter.Calculate();
        builder.Path = path;
        builder.Name = name;
        return builder.Build();
    }

    public static DocumentStatsBuilder CreateBuilder() {
        return new DocumentStatsBuilder((name, path, wordsTF) => new DocumentStats(name, path, wordsTF));
    }
}