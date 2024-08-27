namespace Server;

using System.Text.Json.Serialization;

public struct DatabaseRequest {

    [JsonPropertyName("command")]
    public string Command { get; }

    [JsonConstructor]
    public DatabaseRequest(string command) {
        Command = command;
    }
}