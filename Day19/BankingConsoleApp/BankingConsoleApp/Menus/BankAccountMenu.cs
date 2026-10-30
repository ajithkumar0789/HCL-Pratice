
using System.Text.RegularExpressions;
using BankingConsoleApp.Model;
using BankingConsoleApp.Services;

namespace BankingConsoleApp.Menus;

public class BankAccountMenu
{
    private readonly BankAccountService _accountService;
    private readonly CustomerService _customerService;

    public BankAccountMenu(
        BankAccountService accountService,
        CustomerService customerService)
    {
        _accountService = accountService;
        _customerService = customerService;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== BANK ACCOUNT MANAGEMENT =====");
            Console.WriteLine("1. Add Account");
            Console.WriteLine("2. View Accounts");
            Console.WriteLine("3. Search Account");
            Console.WriteLine("4. Deposit");
            Console.WriteLine("5. Withdraw");
            Console.WriteLine("6. Delete Account");
            Console.WriteLine("7. Back");

            switch (ConsoleInput.ReadText("Enter choice: "))
            {
                case "1": await AddAsync(); break;
                case "2": await ViewAsync(); break;
                case "3": await SearchAsync(); break;
                case "4": await DepositAsync(); break;
                case "5": await WithdrawAsync(); break;
                case "6": await DeleteAsync(); break;
                case "7": return;
                default: Console.WriteLine("Invalid choice."); break;
            }

            ConsoleInput.Pause();
        }
    }

    private async Task AddAsync()
    {
        int customerId = ConsoleInput.ReadInt("Customer ID: ");
        var customer = await _customerService.GetCustomerByIdAsync(customerId);

        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }

        string number = ConsoleInput.ReadText("Account number (10-16 digits): ");

        if (!Regex.IsMatch(number, @"^\d{10,16}$"))
        {
            Console.WriteLine("Invalid account number.");
            return;
        }

        var accounts = await _accountService.GetAllAccountsAsync();

        if (accounts.Any(a => a.AccountNumber == number))
        {
            Console.WriteLine("Account number already exists.");
            return;
        }

        var account = new BankAccount
        {
            AccountHolder = customer.FullName,
            AccountNumber = number,
            CustomerId = customerId
        };

        await _accountService.AddAccountAsync(account);
        Console.WriteLine($"Account created. ID: {account.BankAccountId}");
    }

    private async Task ViewAsync()
    {
        var accounts = await _accountService.GetAllAccountsAsync();

        foreach (var account in accounts)
        {
            Console.WriteLine(
                $"{account.BankAccountId} | {account.AccountNumber} | " +
                $"{account.AccountHolder} | Balance: {account.Balance:F2}");
        }

        if (accounts.Count == 0)
            Console.WriteLine("No accounts found.");
    }

    private async Task SearchAsync()
    {
        int id = ConsoleInput.ReadInt("Account ID: ");
        var account = await _accountService.GetAccountByIdAsync(id);

        if (account == null)
        {
            Console.WriteLine("Account not found.");
            return;
        }

        Console.WriteLine(
            $"{account.BankAccountId} | {account.AccountNumber} | " +
            $"{account.AccountHolder} | Balance: {account.Balance:F2}");
    }

    private async Task DepositAsync()
    {
        int id = ConsoleInput.ReadInt("Account ID: ");
        decimal amount = ConsoleInput.ReadAmount("Deposit amount: ");

        await _accountService.DepositAsync(id, amount);
        Console.WriteLine("Deposit successful.");
    }

    private async Task WithdrawAsync()
    {
        int id = ConsoleInput.ReadInt("Account ID: ");
        decimal amount = ConsoleInput.ReadAmount("Withdrawal amount: ");

        await _accountService.WithdrawAsync(id, amount);
        Console.WriteLine("Withdrawal successful.");
    }

    private async Task DeleteAsync()
    {
        int id = ConsoleInput.ReadInt("Account ID: ");
        await _accountService.DeleteAccountAsync(id);
        Console.WriteLine("Delete request completed.");
    }
}
