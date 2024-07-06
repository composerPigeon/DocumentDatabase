namespace DatabaseNS.Components;

internal class DocumentStats {

    // Word and TermFreq
    public Dictionary<string, double> WordsTF { get;}
    public ComponentName DocumentName { get; set; }

    public DocumentStats(ComponentName documentName, Dictionary<string, double> wordsTF) {
        DocumentName = documentName;
        WordsTF = wordsTF;
    }

}