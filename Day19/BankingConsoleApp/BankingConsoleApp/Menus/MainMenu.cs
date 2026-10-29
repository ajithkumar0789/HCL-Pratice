
namespace BankingConsoleApp.Menus;

public class MainMenu
{
    public void Display()
    {
        Console.WriteLine("\n===== BANKING CONSOLE APPLICATION =====");
        Console.WriteLine("1. Add Customer");
        Console.WriteLine("2. View All Customers");
        Console.WriteLine("3. Add Bank Account");
        Console.WriteLine("4. View All Accounts");
        Console.WriteLine("5. Deposit Money");
        Console.WriteLine("6. Withdraw Money");
        Console.WriteLine("7. Exit");
        Console.Write("Enter your choice: ");
    }
}
