namespace DatabaseNS;

using DatabaseNS.Drivers;

class Program
{
    static void Main(string[] args)
    {
        DatabaseDriver database = DatabaseDriver.InitializeDriver(DriverType.Shell);

        while (true) {
            string? input = Console.ReadLine();

            if (input != null) {
                var result = database.Execute(input);
                Console.WriteLine(result);
                Console.WriteLine("------------------------------");
            }
        }
    }
}
