
namespace BankingConsoleApp.Model;

public class BankAccount
{
    public string AccountHolder { get; set; }
    public string AccountNumber { get; set; }
    public decimal Balance { get; private set; }

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

