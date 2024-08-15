namespace Server;

using DatabaseNS.Drivers;
using DatabaseNS.ResultNS;

public class Program
{
    private static DatabaseDriver _driver = DatabaseDriver.InitializeDriver(DriverType.Server);
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/", () => "Hello World!");

        app.MapPost("/query", PostQuery);

        app.Run();
    }

    public static async Task<IResult> PostQuery(HttpRequest request) {
        string body = await new StreamReader(request.Body).ReadToEndAsync();
        Result result = _driver.Execute(body);
        Console.WriteLine($"Command: {body}");
        Console.WriteLine(result);
        Console.WriteLine("------------------------------");
        switch (result.Type) {
            case ResultType.Ok:
                return Results.Ok(result.Message);
            case ResultType.BadRequest:
                return Results.BadRequest(result.Message);
            case ResultType.InternalServerError:
                return Results.StatusCode(500);
            default:
                return Results.StatusCode(500);
        }
    }

    /**
    public static async Task<IResult> GetQuery(HttpRequest request) {
        string query = await new StreamReader(request.Body).ReadToEndAsync();
        if (query.Length == 0)
            return Results.BadRequest("ERROR: Query is missing");
        else
            return Results.Ok(query);
    }
    **/
}
