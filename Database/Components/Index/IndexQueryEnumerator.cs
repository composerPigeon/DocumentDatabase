namespace DatabaseNS.Components.IndexNS;

using System.Collections;
using DatabaseNS.Components.Values;

// Implementation od enumerating the relative documents and calculates its scores which are relative to inputtted query
internal class IndexQueryEnumerator : IEnumerator<IndexQueryRecord> {

    private SortedList<ComponentName, double>[] _termLists;
    private int[] _inxs;

    private IndexQueryRecord? _last;

    public IndexQueryEnumerator(SortedList<ComponentName, double>[] termLists) {
        _termLists = termLists;
        _inxs = new int[termLists.Length];
    }

    public void Reset() {
        for (int i = 0; i < _inxs.Length; i++) {
            _inxs[i] = 0;
        }
        _last = null;
    }

    public bool MoveNext() {
        _last = getRecordForMinDocument();
        if (_last != null) {
            for (int i = 0; i < _inxs.Length; i++) {
                if (_last.Value.Values[i] > 0) {
                    _inxs[i] += 1;
                }
            }
            return true;
        }

        return false;
    }

    // find current document with minimal name (documents are sorted by their names) 
    private Tuple<ComponentName?, int> findMinDocument() {
        ComponentName? name = null;
        int index = -1;
        for (int i = 0; i < _inxs.Length; i++) {
            int actualIndex = _inxs[i];

            if (actualIndex < _termLists[i].Count) {
                ComponentName actualName = _termLists[i].GetKeyAtIndex(actualIndex);

                if (name == null) {
                    name = actualName;
                    index = i;
                } else {
                    if (actualName.CompareTo(name.Value) < 0) {
                        name = actualName;
                        index = i;
                    }
                }
            }
        }

        if (name != null)
            return new Tuple<ComponentName?, int>(name, index);
        else
            return new Tuple<ComponentName?, int>(null, index);
    }

    // For minimal document then create the vector and fill it with term frequiencies
    private void fillRestValues(List<double> values, int minDocIndex, ComponentName minDocName) {
        for (int i = minDocIndex; i < _inxs.Length; i++) {
            int actualIndex = _inxs[i];
            if (actualIndex < _termLists[i].Count) {
                ComponentName actualName = _termLists[i].GetKeyAtIndex(actualIndex);

                if (minDocName.Equals(actualName))
                    values.Add(_termLists[i].GetValueAtIndex(actualIndex));
                else
                    values.Add(0);

            } else {
                values.Add(0);
            }
        }
    }

    // Create IndexQueryRecord for current minimal document and its vector
    private IndexQueryRecord? getRecordForMinDocument() {
        (ComponentName? minDocName, int minDocIndex) = findMinDocument();

        if (minDocName == null)
            return null;
        else {
            List<double> values = Enumerable.Repeat(0.0, minDocIndex).ToList();
            fillRestValues(values, minDocIndex, minDocName.Value);
            return new IndexQueryRecord(minDocName.Value, values);
        }
    }

    void IDisposable.Dispose() {}

    object IEnumerator.Current {
        get { return Current; }
    }

    public IndexQueryRecord Current {
        get {
            if (_last.HasValue)
                return _last.Value;
            else
                throw new InvalidOperationException();
        }
    }

}