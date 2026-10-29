namespace BankingConsoleApp.Helper;

public static class ErrorHandler
{
    public static void Handle(Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}