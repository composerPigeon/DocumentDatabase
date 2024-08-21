namespace Server;

using System.Text.Json.Serialization;
using System.Text.Json;

public class DatabaseRequest {

    [JsonPropertyName("command")]
    public string Command { get; }

    [JsonConstructor]
    public DatabaseRequest(string command) {
        Command = command;
    }
}