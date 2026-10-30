
namespace BankingConsoleApp.Menus;

public static class ConsoleInput
{
    public static string ReadText(string message)
    {
        while (true)
        {
            Console.Write(message);
            string? value = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(value))
                return value.Trim();

            Console.WriteLine("This field is required.");
        }
    }

    public static int ReadInt(string message)
    {
        while (true)
        {
            string value = ReadText(message);

            if (int.TryParse(value, out int result) && result > 0)
                return result;

            Console.WriteLine("Enter a valid positive number.");
        }
    }

    public static decimal ReadAmount(string message)
    {
        while (true)
        {
            string value = ReadText(message);

            if (decimal.TryParse(value, out decimal result) &&
                result > 0 &&
                decimal.Round(result, 2) == result)
                return result;

            Console.WriteLine("Enter a valid positive amount.");
        }
    }

    public static void Pause()
    {
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }
}
