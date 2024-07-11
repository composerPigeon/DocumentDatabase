namespace DatabaseNS.Components.IndexNS;

internal struct IndexRecord : IComparable<IndexRecord> {
    public ComponentName DocumentName {get;}
    public double Score {get;}

    public int CompareTo(IndexRecord other) {
        return -Score.CompareTo(other.Score);
    }

    public IndexRecord(ComponentName name, double score) {
        DocumentName = name;
        Score = score;
    }
}