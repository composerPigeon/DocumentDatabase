namespace DatabaseNS.Components.IndexNS;

using System.Collections;
using DatabaseNS.Components.Values;

// Unit used when iterating over the IndexQuery. Used for more type information
internal struct IndexQueryRecord {
    public ComponentName Name {get;}
    public double[] Values { get;}

    public IndexQueryRecord(ComponentName name, List<double> values) {
        Name = name;
        Values = values.ToArray();
    }
}

// class which projects relative documents from the index and implements ienumrable so the result can be iterated
internal class IndexQuery : IEnumerable<IndexQueryRecord> {

    SortedList<ComponentName, double>[] _termLists;

    public IndexQuery(string[] keyWords, Dictionary<string, SortedList<ComponentName, double>> wordByDocumentTF) {
        var termLists = new List<SortedList<ComponentName, double>>();
        foreach (var term in keyWords) {
            if (wordByDocumentTF.ContainsKey(term)) {
                termLists.Add(wordByDocumentTF[term]);
            } else {
                termLists.Add(new SortedList<ComponentName, double>());
            }
        }
        _termLists = termLists.ToArray();
    }

    public IEnumerator<IndexQueryRecord> GetEnumerator() {
        return new IndexQueryEnumerator(_termLists);
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return this.GetEnumerator();
    }
}