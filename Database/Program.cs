namespace DatabaseNS;

using DatabaseNS.Drivers;
using DatabaseNS.ResultNS;
using DatabaseNS.ResultNS.LoggerNS;

// Program for console application
class Program
{

    private static DatabaseLogger _logger = new DatabaseLogger(Console.Out);
    static void Main(string[] args)
    {
        DatabaseDriver database = DatabaseDriver.InitializeDriver(DriverType.Shell);

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
