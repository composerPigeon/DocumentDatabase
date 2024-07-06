namespace DatabaseNS.DocumentParserNS;

using DatabaseNS.Components;

internal class DocumentStatsBuilder {
    private Dictionary<string, ulong> _wordCounts;
    private ulong _maxCount;

    public DocumentStatsBuilder() {
        _wordCounts = new Dictionary<string, ulong>();
        _maxCount = 0;
    }

    public void AddWord(string word) {
        ulong count;
        if (_wordCounts.ContainsKey(word)) {
            _wordCounts[word] += 1;
            count = _wordCounts[word];
        } else {
            _wordCounts[word] = 1;
            count = 1;
        }

        if (count > _maxCount)
            _maxCount = count;
    }

    public DocumentStats Build(ComponentName documentName) {
        var wordsTF = new Dictionary<string, double>();

        foreach(var entry in _wordCounts) {
            wordsTF.Add(entry.Key, (double)entry.Value / _maxCount);
        }
        return new DocumentStats(
            documentName,
            wordsTF
        );
    }
}