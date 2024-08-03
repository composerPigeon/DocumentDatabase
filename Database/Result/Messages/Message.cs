namespace DatabaseNS.ResultNS.Messages;

public struct Message {

    private string _value;

    public Message(string value) {
        _value = value;
    }

    public override string ToString() {
        return _value;
    }
}