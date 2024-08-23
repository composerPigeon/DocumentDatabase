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

    // Get potential ComponentName of this path
    public ComponentName GetComponentName() {
        if (Directory.Exists(_value))
            return new DirectoryInfo(_value).Name.ToName();
        else if (File.Exists(_value))
            return Path.GetFileNameWithoutExtension(_value).ToName();
        else
            return ComponentName.Empty;
    }

    // Add path to the other
    public ComponentPath AppendPath(ComponentPath path) {
        return new ComponentPath() {
            _value = Path.Combine(this._value, path._value)
        };
    }

    // Add string as next part of path
    public ComponentPath AppendString(string content) {
        return new ComponentPath() {
            _value = Path.Combine(_value, content)
        };
    }

    // Add name as next part of path
    public ComponentPath AppendName(ComponentName name) {
        return new ComponentPath() {
            _value = Path.Combine(this._value, name.ToString())
        };
    }

    public override string ToString() {
        return _value;
    }

    // Implicitly converts to string, so it can be used without conversion in cases where default C# methods expects string as path fo files or directories
    public static implicit operator string(ComponentPath path) {
        return path._value;
    }
}

// Extension for string to convert to path
internal static class StringToComponentPathExtensions {
    public static ComponentPath AsPath(this string value) {
        return new ComponentPath(value);
    }
}

// Serializing ComponentPath as string when parsing to json
internal class ComponentPathJsonConverter : JsonConverter<ComponentPath>
{
    public override ComponentPath Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        return new ComponentPath(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, ComponentPath value, JsonSerializerOptions options) {
        writer.WriteStringValue(value.ToString());
    }
}