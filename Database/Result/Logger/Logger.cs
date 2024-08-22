namespace DatabaseNS.ResultNS.LoggerNS;

using System.Text.Json;
using DatabaseNS.ResultNS;
using DatabaseNS.ResultNS.Messages;

// Class for logging results
public class DatabaseLogger {
    private TextWriter _writer;
    public DatabaseLogger(TextWriter writer) {
        _writer = writer;
    }

    public void LogInfo(string? command, Result result) {
        string content = JsonSerializer.Serialize(new LoggerRecord() {
            Type = "Info",
            Command = command,
            Result = result
        }, Result.JsonSerializerOptions);
        _writer.WriteLine(content);
    }

    public void LogInfo(Message message) {
        _writer.WriteLine($"Info> {message}");
    }

    public void LogError(string? command, Result result) {
        string content = JsonSerializer.Serialize(new LoggerRecord() {
            Type = "Error",
            Command = command,
            Result = result
        }, Result.JsonSerializerOptions);
        _writer.WriteLine(content);
    }

    public void LogError(Message message) {
        _writer.WriteLine($"Error> {message}");
    }

    private class LoggerRecord {
        public string? Type { get; init; }
        public string? Command { get; init; }
        public Result Result { get; init; }
    }

}