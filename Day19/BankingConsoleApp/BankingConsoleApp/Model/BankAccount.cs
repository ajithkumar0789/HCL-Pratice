
namespace BankingConsoleApp.Model;

public class BankAccount
{
    public int BankAccountId { get; set; }

    public string AccountHolder { get; set; } = string.Empty;

    public string AccountNumber { get; set; } = string.Empty;

    public decimal Balance { get; private set; }

    // Customer foreign key
    public int CustomerId { get; set; }

    // Customer navigation property
    public Customer? Customer { get; set; }

    // Branch foreign key (optional)
    public int? BranchId { get; set; }

    // Branch navigation property
    public Branch? Branch { get; set; }

    // One account can have multiple transactions
    public List<Transaction> Transactions { get; set; } = new();

    public BankAccount()
    {
    }

    public BankAccount(string holder, string number)
    {
        AccountHolder = holder;
        AccountNumber = number;
        Balance = 0;
    }

    public bool Deposit(decimal amount)
    {
        if (amount <= 0)
            return false;

        Balance += amount;
        return true;
    }

    public bool Withdraw(decimal amount)
    {
        if (amount <= 0 || amount > Balance)
            return false;

        Balance -= amount;
        return true;
    }
}
