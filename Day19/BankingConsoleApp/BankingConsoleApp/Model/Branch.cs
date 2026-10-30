
namespace BankingConsoleApp.Model;

public class Branch
{
    public int BranchId { get; set; }

    public string BranchName { get; set; } = string.Empty;

    public string BranchCode { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    // One branch can have many bank accounts
    public List<BankAccount> BankAccounts { get; set; } = new();

    // One branch can have many employees
    public List<Employee> Employees { get; set; } = new();
}
