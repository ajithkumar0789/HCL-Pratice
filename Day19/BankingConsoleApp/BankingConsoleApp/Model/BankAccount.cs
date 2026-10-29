
namespace BankingConsoleApp.Model;

public class BankAccount
{
    public int BankAccountId { get; set; }

    public string AccountHolder { get; set; } = string.Empty;

    public string AccountNumber { get; set; } = string.Empty;

    public decimal Balance { get; private set; }

    // Foreign key
    public int CustomerId { get; set; }

    // Navigation property
    public Customer? Customer { get; set; }

    public BankAccount(string holder, string number)
    {
        AccountHolder = holder;
        AccountNumber = number;
        Balance = 0;
    }

    public BankAccount()
    {
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
