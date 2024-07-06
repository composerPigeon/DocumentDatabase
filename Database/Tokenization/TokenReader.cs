namespace DatabaseNS.Tokenization;

internal enum ReaderState {
    InWord,
    InContent,
    Out
}

internal interface TokenReader {
    public Token Read();
}