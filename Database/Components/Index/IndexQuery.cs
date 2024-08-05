namespace DatabaseNS.Components.IndexNS;

using System.Collections;
using DatabaseNS.FileSystem;

internal struct IndexQueryRecord {
    public ComponentName Name {get;}
    public double[] Values { get;}

    public IndexQueryRecord(ComponentName name, List<double> values) {
        Name = name;
        Values = values.ToArray();
    }
}

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