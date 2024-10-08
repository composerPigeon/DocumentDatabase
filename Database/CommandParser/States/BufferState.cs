namespace DatabaseNS.CommandParserNS.States;

using DatabaseNS.Components.Values;
using DatabaseNS.ResultNS.Handlers;
using DatabaseNS.Tokenization;

// State which expects to find some words which sould be added to internal state buffer. This buffer is then handled when the state is changing to different one 
internal abstract class BufferState : State {

    private int _maxBufferCount;
    private Queue<Token> _buffer;

    protected int bufferCount {
        get {
            return _buffer.Count;
        }
    }

    protected BufferState(State state, BufferType countType) : base(state) {
        _buffer = new Queue<Token>();

        switch (countType) {
            case BufferType.One:
                _maxBufferCount = 1;
                break;
            default:
                _maxBufferCount = 20;
                break;
        }
        
    }

    protected void addTokenToBuffer(Token token) {
        if (bufferCount + 1 > _maxBufferCount)
            throw Handlers.Exception.ThrowCommandParseInvalidToken(token);
        _buffer.Enqueue(token);
    }

    protected Token getTokenFromBuffer() {
        return _buffer.Dequeue();
    }

    protected ComponentName? getNameFromBuffer() {
        Token t = _buffer.Dequeue();
        if (!t.IsLast && t.Word != null) {
            return t.Word.ToName();
        }
        throw Handlers.Exception.ThrowCommandParseInvalidToken(t);
    }

    protected List<string> getContentFromBuffer() {
        return _buffer.Select(x => x.Word ?? "").Where(x => x != "").ToList();
    }
}

internal enum BufferType {
    One,
    Any
}