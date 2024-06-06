namespace Database_DocumentParser;

using Database_DbComponents;
using Database_Tokenization;

internal class DocumentParser {
    public static HashSet<string>? StopWords;

    public static DocumentStats Parse(ComponentPath path, ComponentName documentName) {
        TokenReader reader = new DocumentTokenReader(path);
        DocumentStatsBuilder builder = new DocumentStatsBuilder();

        Token token = reader.Read();
        while (!token.IsLast) {
            if (token.Word != null)
                if (StopWords != null) {
                    if (!StopWords.Contains(token.Word))
                        builder.AddWord(token.Word);
                } else {
                    builder.AddWord(token.Word);
                }
            token = reader.Read();
        }
        return builder.Build(documentName);
    }
}