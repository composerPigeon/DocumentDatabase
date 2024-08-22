namespace DatabaseNS.ResultNS.LoggerNS;

using System.Text.Json;
using DatabaseNS.ResultNS;

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

    public void LogError(string? command, Result result) {
        string content = JsonSerializer.Serialize(new LoggerRecord() {
            Type = "Error",
            Command = command,
            Result = result
        }, Result.JsonSerializerOptions);
        _writer.WriteLine(content);
    }

    private class LoggerRecord {
        public string? Type { get; init; }
        public string? Command { get; init; }
        public Result Result { get; init; }
    }

}