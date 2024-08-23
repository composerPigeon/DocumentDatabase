namespace Server;

using System.Text.Json;
using DatabaseNS.Drivers;
using DatabaseNS.ResultNS;
using DatabaseNS.ResultNS.LoggerNS;
using DatabaseNS.ResultNS.Messages;

public class Program
{
    private static DatabaseLogger _logger = new DatabaseLogger(Console.Out);
    public static void Main(string[] args)
    {
        if (args.Length >= 1) {
            if (args[0] == "server")
                runServer(args[1..]);
            else if (args[0] == "console")
                runShell();
            else
                _logger.LogError(ErrorMessages.InvalidFirstArgument(args[0]));

        } else {
            _logger.LogError(ErrorMessages.NoArgumentProvided());
        }
    }

    private static void runServer(string[] args) {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();
        var driver = DatabaseDriver.InitializeDriver(DriverType.Server);

        app.MapPost("/", async (HttpRequest request) => {
            string body = await new StreamReader(request.Body).ReadToEndAsync();
            DatabaseRequest dbRequest;
            try {
                dbRequest = JsonSerializer.Deserialize<DatabaseRequest>(body)!;
            } catch (JsonException) {
                return Results.BadRequest(new {Error = ErrorMessages.InvalidJsonOnEndpoint(body).ToString()});
            }
            
            Result result = driver.Execute(dbRequest.Command);
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

    private static void runShell() {
        DatabaseDriver database = DatabaseDriver.InitializeDriver(DriverType.Shell);
        _logger.LogInfo(CorrectMessages.ConsoleReady());

        while (true) {
            string? input = Console.ReadLine();

            if (input != null) {
                var result = database.Execute(input);
                switch (result.Type) {
                    case ResultType.Ok:
                        _logger.LogInfo(result.Message);
                        break;
                    default:
                        _logger.LogError(result.Message);
                        break;
                }
            }
        }
    }
 }
