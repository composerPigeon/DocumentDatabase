namespace Server;

using System.Text.Json;
using DatabaseNS.Drivers;
using DatabaseNS.ResultNS;
using DatabaseNS.ResultNS.LoggerNS;

public class Program
{
    private static DatabaseDriver _driver = DatabaseDriver.InitializeDriver(DriverType.Server);
    private static DatabaseLogger _logger = new DatabaseLogger(Console.Out);
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapPost("/", async (HttpRequest request) => {
            string body = await new StreamReader(request.Body).ReadToEndAsync();
            DatabaseRequest dbRequest = JsonSerializer.Deserialize<DatabaseRequest>(body)!;
            Result result = _driver.Execute(dbRequest.Command);
            switch (result.Type) {
                case ResultType.Ok:
                    _logger.LogInfo(dbRequest.Command, result);
                    return Results.Ok(new { Ok = result.Message.ToString() });
                case ResultType.BadRequest:
                    _logger.LogError(dbRequest.Command, result);
                    return Results.BadRequest(new { Error = result.Message.ToString() });
                default:
                    _logger.LogError(dbRequest.Command, result);
                    return Results.StatusCode(500);
            }
        });

        app.Run();
    }
}
