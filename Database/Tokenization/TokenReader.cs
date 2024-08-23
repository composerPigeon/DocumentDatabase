namespace DatabaseNS.Tokenization;

internal enum ReaderState {
    InWord,
    InContent,
    Out
}

// Abstract class which represents token reader of any type, implements IDisposable
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