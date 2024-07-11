using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using DatabaseNS.Components.Builders;
using DatabaseNS.DocumentParserNS;

namespace DatabaseNS.Components;

internal class DocumentStats : DatabaseComponent, IComponentBuildable<DocumentStats, DocumentStatsBuilder> {

    // Word and TermFreq
    public Dictionary<string, double> WordsTF { get;}

    [JsonConstructor]
    private DocumentStats(ComponentName name, ComponentPath path, Dictionary<string, double> wordsTF) : base(name, path) {
        WordsTF = wordsTF;
    }

    public void Save(JsonSerializerOptions options) {
        string content = JsonSerializer.Serialize(this, options);
        Path.Write(content);
    }

    public void Remove() {
        Path.Remove();
    }

    public static DocumentStats ReadDocument(ComponentName name, ComponentPath path, string content) {
        WordCounter counter = DocumentParser.Parse(content);
        DocumentStatsBuilder builder = new DocumentStatsBuilder {
            WordsTF = counter.Calculate(),
            Path = path,
            Name = name
        };
        return BuildFrom(builder);
    }

    public static DocumentStats BuildFrom(DocumentStatsBuilder builder) {
        if (builder.WordsTF != null && builder.Name.HasValue && builder.Path.HasValue) {
            return new DocumentStats(
                builder.Name.Value,
                builder.Path.Value,
                builder.WordsTF
            );
        } else
            throw new DatabaseCreateException(ErrorMessages.STATS_CREATE);
    }
}