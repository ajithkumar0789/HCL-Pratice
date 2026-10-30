
namespace BankingConsoleApp.Model;

public class Loan
{
    public int LoanId { get; set; }

    public string LoanType { get; set; } = string.Empty;

    public decimal LoanAmount { get; set; }

    public decimal InterestRate { get; set; }

    public DateTime LoanDate { get; set; } = DateTime.Now;

    // Pending, Approved, or Rejected
    public string Status { get; set; } = "Pending";

    // Foreign key
    public int CustomerId { get; set; }

    // Navigation property
    public Customer? Customer { get; set; }
}
