namespace DatabaseNS;

using DatabaseNS.Components;

class Program
{
    static void Main(string[] args)
    {
        DatabaseDriver database = new ShellDatabaseDriver("Data");

        while (true) {
            string? input = Console.ReadLine();

            if (input != null) {
                Result result = database.Execute(input);
                Console.WriteLine(result);
                if (result.Action != null)
                    result.Action(0);
            }
        }
    }
}
