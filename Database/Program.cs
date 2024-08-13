namespace DatabaseNS;

using DatabaseNS.FileSystem;
using DatabaseNS.ResultNS;
using DatabaseNS.Drivers;

class Program
{
    static void Main(string[] args)
    {
        DatabaseDriver database = DatabaseDriver.Instance;

        Console.WriteLine(EntryCreator.DataPath);

        while (true) {
            string? input = Console.ReadLine();

            if (input != null) {
                var result = database.Execute(input);
                Console.Write(result.Type.GetString());
                Console.WriteLine(": ");
                Console.WriteLine(result.Message);
            }
        }
    }
}
