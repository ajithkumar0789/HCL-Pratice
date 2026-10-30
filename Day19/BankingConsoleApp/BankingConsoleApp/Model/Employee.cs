
namespace BankingConsoleApp.Model;

public class Employee
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Designation { get; set; } = string.Empty;

    // Foreign key
    public int BranchId { get; set; }

    // Navigation property
    public Branch? Branch { get; set; }
}
