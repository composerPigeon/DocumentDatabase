namespace DatabaseNS.DocumentParserNS;


// Incrementally calculates absolute term frequiencies 
internal class WordCounter {
    private Dictionary<string, ulong> _wordCounts;
    private ulong _maxCount;

    public WordCounter() {
        _wordCounts = new Dictionary<string, ulong>();
        _maxCount = 0;
    }

    // increments frequency of words
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

    // Parse absolute frequiencies to realitve one, which are used later in inverted index
    public Dictionary<string, double> Calculate() {
        var counts = new Dictionary<string, double>();

        foreach(string word in _wordCounts.Keys) {
            double score = (double)_wordCounts[word] / _maxCount;
            counts.Add(word, score);
        }

        return counts;
    }
}