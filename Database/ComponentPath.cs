namespace DatabaseNS;

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

    public TextReader GetReader() {
        return new StreamReader(_value);
    }

    public IEnumerable<ComponentPath> List() {
        if (Directory.Exists(_value))
            foreach (var path in Directory.EnumerateFileSystemEntries(_value)) {
                yield return new ComponentPath(path);
            }
        else
            throw new DirectoryNotFoundException();
    }

    public void Write(string content) {
        using (var writer = new StreamWriter(_value)) {
            writer.Write(content);
        }
    }

    public void Remove() {
        try {
            if (Directory.Exists(_value)) {
                Directory.Delete(_value, true);
            } else {
                File.Delete(_value);
            }
        } catch (DirectoryNotFoundException) {}
    }

    public T? LoadAsJson<T>(JsonSerializerOptions options) {
        string content = File.ReadAllText(_value);
        return JsonSerializer.Deserialize<T>(content, options);
    }

    public override string ToString() {
        return _value;
    }

    public static ComponentPath operator +(ComponentPath left, ComponentPath right) {
        return new ComponentPath() {
            _value = Path.Combine(left._value, right._value)
        };
    }

    public static ComponentPath operator +(ComponentPath left, ComponentName right) {
        return new ComponentPath() {
            _value = Path.Combine(left._value, right.ToString())
        };
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