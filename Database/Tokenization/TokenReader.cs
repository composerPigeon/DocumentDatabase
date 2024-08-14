namespace DatabaseNS.Tokenization;

internal enum ReaderState {
    InWord,
    InContent,
    Out
}

internal abstract class TokenReader : IDisposable {
    protected TextReader _reader;

    protected TokenReader(TextReader reader) {
        _reader = reader;
    }

    public void Dispose() {
        _reader.Dispose();
    }
    public abstract Token Read();
}