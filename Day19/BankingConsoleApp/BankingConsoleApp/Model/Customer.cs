
namespace BankingConsoleApp.Model;

public class Customer
{
    public int CustomerId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    // One customer can have multiple bank accounts
    public List<BankAccount> BankAccounts { get; set; } = new();

    // One customer can have multiple loans
    public List<Loan> Loans { get; set; } = new();
}