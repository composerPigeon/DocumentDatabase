namespace DatabaseNS.Components.Values;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

[JsonConverter(typeof(ComponentPathJsonConverter))]
internal struct ComponentPath {
    private string _value { get; init; }

    public ComponentPath(string value) {
        _value = value;
    }

    public bool EndsWith(string value) {
        return _value.EndsWith(value);
    }

    public ComponentName GetComponentName() {
        if (Directory.Exists(_value))
            return new DirectoryInfo(_value).Name.ToName();
        if (File.Exists(_value))
            return new FileInfo(_value).Name.ToName();
        else
            return ComponentName.Empty;
    }

    public ComponentPath AppendPath(ComponentPath path) {
        return new ComponentPath() {
            _value = Path.Combine(this._value, path._value)
        };
    }

    public ComponentPath AppendString(string content) {
        return new ComponentPath() {
            _value = Path.Combine(_value, content)
        };
    }

    public ComponentPath AppendName(ComponentName name) {
        return new ComponentPath() {
            _value = Path.Combine(this._value, name.ToString())
        };
    }

    public static implicit operator string(ComponentPath path) {
        return path._value;
    }
}

internal static class StringToComponentPathExtensions {
    public static ComponentPath AsPath(this string value) {
        return new ComponentPath(value);
    }
}

internal class ComponentPathJsonConverter : JsonConverter<ComponentPath>
{
    public override ComponentPath Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        return new ComponentPath(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, ComponentPath value, JsonSerializerOptions options) {
        writer.WriteStringValue(value.ToString());
    }
}