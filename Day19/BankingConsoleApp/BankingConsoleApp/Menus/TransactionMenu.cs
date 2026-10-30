
using BankingConsoleApp.Model;
using BankingConsoleApp.Services;

namespace BankingConsoleApp.Menus;

public class TransactionMenu
{
    private readonly TransactionService _service;
    private readonly BankAccountService _accountService;

    public TransactionMenu(
        TransactionService service,
        BankAccountService accountService)
    {
        _service = service;
        _accountService = accountService;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== TRANSACTION MANAGEMENT =====");
            Console.WriteLine("1. Add Transaction");
            Console.WriteLine("2. View Transactions");
            Console.WriteLine("3. Search Transaction");
            Console.WriteLine("4. Update Transaction");
            Console.WriteLine("5. Delete Transaction");
            Console.WriteLine("6. Back");

            switch (ConsoleInput.ReadText("Enter choice: "))
            {
                case "1": await AddAsync(); break;
                case "2": await ViewAsync(); break;
                case "3": await SearchAsync(); break;
                case "4": await UpdateAsync(); break;
                case "5": await DeleteAsync(); break;
                case "6": return;
                default: Console.WriteLine("Invalid choice."); break;
            }

            ConsoleInput.Pause();
        }
    }

    private async Task AddAsync()
    {
        int accountId = ConsoleInput.ReadInt("Bank account ID: ");

        var account = await _accountService.GetAccountByIdAsync(accountId);

        if (account == null)
        {
            Console.WriteLine("Account not found.");
            return;
        }

        string type = ConsoleInput.ReadText("Type (Deposit/Withdrawal): ");

        if (type.Equals("Deposit", StringComparison.OrdinalIgnoreCase))
            type = "Deposit";
        else if (type.Equals("Withdrawal", StringComparison.OrdinalIgnoreCase))
            type = "Withdrawal";
        else
        {
            Console.WriteLine("Invalid transaction type.");
            return;
        }

        var transaction = new Transaction
        {
            BankAccountId = accountId,
            Amount = ConsoleInput.ReadAmount("Amount: "),
            TransactionType = type,
            TransactionDate = DateTime.Now
        };

        await _service.AddTransactionAsync(transaction);
        Console.WriteLine($"Transaction added. ID: {transaction.TransactionId}");
    }

    private async Task ViewAsync()
    {
        var transactions = await _service.GetAllTransactionsAsync();

        foreach (var transaction in transactions)
            Console.WriteLine($"{transaction.TransactionId} | Account: {transaction.BankAccountId} | {transaction.TransactionType} | {transaction.Amount:F2} | {transaction.TransactionDate}");

        if (transactions.Count == 0)
            Console.WriteLine("No transactions found.");
    }

    private async Task SearchAsync()
    {
        int id = ConsoleInput.ReadInt("Transaction ID: ");
        var transaction = await _service.GetTransactionByIdAsync(id);

        if (transaction == null)
        {
            Console.WriteLine("Transaction not found.");
            return;
        }

        Console.WriteLine($"{transaction.TransactionId} | Account: {transaction.BankAccountId} | {transaction.TransactionType} | {transaction.Amount:F2}");
    }

    private async Task UpdateAsync()
    {
        int id = ConsoleInput.ReadInt("Transaction ID: ");
        var transaction = await _service.GetTransactionByIdAsync(id);

        if (transaction == null)
        {
            Console.WriteLine("Transaction not found.");
            return;
        }

        transaction.Amount = ConsoleInput.ReadAmount("New amount: ");

        string type = ConsoleInput.ReadText("New type (Deposit/Withdrawal): ");

        if (type.Equals("Deposit", StringComparison.OrdinalIgnoreCase))
            transaction.TransactionType = "Deposit";
        else if (type.Equals("Withdrawal", StringComparison.OrdinalIgnoreCase))
            transaction.TransactionType = "Withdrawal";
        else
        {
            Console.WriteLine("Invalid transaction type.");
            return;
        }

        await _service.UpdateTransactionAsync(transaction);
        Console.WriteLine("Transaction updated.");
    }

    private async Task DeleteAsync()
    {
        int id = ConsoleInput.ReadInt("Transaction ID: ");
        await _service.DeleteTransactionAsync(id);
        Console.WriteLine("Delete request completed.");
    }
}
