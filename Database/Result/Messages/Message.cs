using System.Text.Json;
using System.Text.Json.Serialization;

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

//Json converter for message (converts it and reads it as string)
internal class MessageJsonConverter : JsonConverter<Message>
{
    public override Message Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        return new Message(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, Message value, JsonSerializerOptions options) {
        writer.WriteStringValue(value.ToString());
    }
}