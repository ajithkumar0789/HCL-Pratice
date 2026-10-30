
namespace BankingConsoleApp.Model;

public class Transaction
{
    public int TransactionId { get; set; }

    public decimal Amount { get; set; }

    // Deposit or Withdrawal
    public string TransactionType { get; set; } = string.Empty;

    public DateTime TransactionDate { get; set; } = DateTime.Now;

    // Foreign key
    public int BankAccountId { get; set; }

    // Navigation property
    public BankAccount? BankAccount { get; set; }
}
