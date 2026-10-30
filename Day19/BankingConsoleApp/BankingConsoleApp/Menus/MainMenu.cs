
namespace BankingConsoleApp.Menus;

public class MainMenu
{
    private readonly CustomerMenu _customerMenu;
    private readonly BankAccountMenu _accountMenu;
    private readonly BranchMenu _branchMenu;
    private readonly EmployeeMenu _employeeMenu;
    private readonly TransactionMenu _transactionMenu;
    private readonly LoanMenu _loanMenu;
    private readonly AuditLogMenu _auditLogMenu;

    public MainMenu(
        CustomerMenu customerMenu,
        BankAccountMenu accountMenu,
        BranchMenu branchMenu,
        EmployeeMenu employeeMenu,
        TransactionMenu transactionMenu,
        LoanMenu loanMenu,
        AuditLogMenu auditLogMenu)
    {
        _customerMenu = customerMenu;
        _accountMenu = accountMenu;
        _branchMenu = branchMenu;
        _employeeMenu = employeeMenu;
        _transactionMenu = transactionMenu;
        _loanMenu = loanMenu;
        _auditLogMenu = auditLogMenu;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("===== BANKING CONSOLE APPLICATION =====");
            Console.WriteLine("1. Customer Management");
            Console.WriteLine("2. Bank Account Management");
            Console.WriteLine("3. Branch Management");
            Console.WriteLine("4. Employee Management");
            Console.WriteLine("5. Transaction Management");
            Console.WriteLine("6. Loan Management");
            Console.WriteLine("7. Audit Log Management");
            Console.WriteLine("8. Exit");

            string choice = ConsoleInput.ReadText("Enter choice: ");

            try
            {
                switch (choice)
                {
                    case "1":
                        await _customerMenu.RunAsync();
                        break;

                    case "2":
                        await _accountMenu.RunAsync();
                        break;

                    case "3":
                        await _branchMenu.RunAsync();
                        break;

                    case "4":
                        await _employeeMenu.RunAsync();
                        break;

                    case "5":
                        await _transactionMenu.RunAsync();
                        break;

                    case "6":
                        await _loanMenu.RunAsync();
                        break;

                    case "7":
                        await _auditLogMenu.RunAsync();
                        break;

                    case "8":
                        Console.WriteLine("Thank you for using Banking App!");
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        ConsoleInput.Pause();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ConsoleInput.Pause();
            }
        }
    }
}
