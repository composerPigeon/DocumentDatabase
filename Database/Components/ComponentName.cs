namespace DatabaseNS.Components;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

internal struct ComponentName : IComparable<ComponentName>, IEquatable<ComponentName> {

    [JsonInclude]
    private string _value { get; init;}

    public ComponentName(string value) {
        _value = value;
    }

    public bool Equals(ComponentName other) {
        return _value == other._value;
    }

    public override bool Equals(object? obj){
        if (obj != null && obj is ComponentName otherName) {
            return Equals(otherName);
        }
        return false;
    }

    public override int GetHashCode() {
        return _value.GetHashCode();
    }

    public override string ToString() {
        return _value;
    }

    public int CompareTo(ComponentName other) {
        return _value.CompareTo(other._value);
    }

    public ComponentPath WithExtension(string extension) {
        return new ComponentPath(_value + extension);
    }

    public ComponentName Concat(string content) {
        return new ComponentName(_value + content);
    }
}

internal class NameToStringAsPropertyConverter : JsonConverter<ComponentName> {
    public override ComponentName ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        return Read(ref reader, typeToConvert, options);
    }

    public override ComponentName Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        return new ComponentName(reader.GetString()!);
    }

    public override void WriteAsPropertyName(Utf8JsonWriter writer, [DisallowNull] ComponentName value, JsonSerializerOptions options) {
        writer.WritePropertyName(value.ToString());
    }

    public override void Write(Utf8JsonWriter writer, ComponentName value, JsonSerializerOptions options) {
        writer.WriteStringValue(value.ToString());
    }
}