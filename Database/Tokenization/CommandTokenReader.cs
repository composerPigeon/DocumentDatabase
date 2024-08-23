namespace DatabaseNS.Tokenization;

using System.Text;

internal class CommandTokenReader : TokenReader {

    private ReaderState _state;
    private char _last;
    StringBuilder _buffer;
    public CommandTokenReader(string command) : base(new StringReader(command)){
        _state = ReaderState.Out;
        _last = '\0';
        _buffer = new StringBuilder();
    }

    // Reads one token from inputted stream
    public override Token Read() {
        int x = _reader.Read();

        while (x != -1) {
            char c = (char)x;

            if (_state == ReaderState.Out) {
                if (c == '{' && _last == '$') {
                    _state = ReaderState.InContent;
                } else if (char.IsLetterOrDigit(c)) {
                    _buffer.Append(c);
                    _state = ReaderState.InWord;
                }
            } else if (_state == ReaderState.InWord) {
                if (!char.IsWhiteSpace(c)) {
                    _buffer.Append(c);
                } else {
                    _state = ReaderState.Out;
                    return new Token(flush(false, true));
                }
            } else {
                if (c == '$' && _last == '}') {
                    _state = ReaderState.Out;
                    return new Token(flush(true, false));
                } else {
                    _buffer.Append(c);
                }
            }
            _last = c;
            x = _reader.Read();
        }

        if (_buffer.Length > 0) {
            return new Token(flush(false, true));
        }
        
        return Token.Last;
    }

    private string flush(bool removeLastChar, bool toLower) {
        if (removeLastChar) {
            _buffer.Remove(_buffer.Length - 1, 1);
        }
        string result = toLower ? _buffer.ToString().ToLower() : _buffer.ToString();
        _buffer.Clear();
        return result;
    }
}