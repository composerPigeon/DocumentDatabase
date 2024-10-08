namespace DatabaseNS.Tokenization;

using System.Text;
using DatabaseNS.Components;

internal class DocumentTokenReader : TokenReader {

    private ReaderState _state;
    StringBuilder _buffer;

    private List<char> _punctuation;

    public DocumentTokenReader(TextReader reader) : base(reader) {
        _state = ReaderState.Out;
        _buffer = new StringBuilder();
        _punctuation = new List<char>(){'.', ',', ':', '!', '?', ';'};
    }

    public DocumentTokenReader(string content) : this(new StringReader(content)) {}

    // Reads one token from inputted stream
    public override Token Read() {
        int x = _reader.Read();

        while (x != -1) {
            char c = (char)x;
            switch (_state) {
                case ReaderState.Out:
                    if (!char.IsWhiteSpace(c) && !_punctuation.Contains(c)) {
                        _buffer.Append(c);
                        _state = ReaderState.InWord;
                    }
                    break;
                case ReaderState.InWord:
                    if (char.IsWhiteSpace(c) || _punctuation.Contains(c)) {
                        _state = ReaderState.Out;
                        return new Token(flush(false));
                    } else {
                        _buffer.Append(c);
                    }
                    break;
            }
            x = _reader.Read();
        }

        if (_buffer.Length > 0) {
            return new Token(flush(false));
        }

        return Token.Last;
    }

    private string flush(bool removeLastChar) {
        if (removeLastChar) {
            _buffer.Remove(_buffer.Length - 1, 1);
        }
        string result = _buffer.ToString().ToLower();
        _buffer.Clear();
        return result;
    }
}