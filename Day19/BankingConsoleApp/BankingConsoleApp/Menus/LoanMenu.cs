
using BankingConsoleApp.Model;
using BankingConsoleApp.Services;

namespace BankingConsoleApp.Menus;

public class LoanMenu
{
    private readonly LoanService _service;
    private readonly CustomerService _customerService;

    public LoanMenu(LoanService service, CustomerService customerService)
    {
        _service = service;
        _customerService = customerService;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== LOAN MANAGEMENT =====");
            Console.WriteLine("1. Add Loan");
            Console.WriteLine("2. View Loans");
            Console.WriteLine("3. Search Loan");
            Console.WriteLine("4. Update Loan");
            Console.WriteLine("5. Delete Loan");
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
        int customerId = ConsoleInput.ReadInt("Customer ID: ");

        var customer = await _customerService.GetCustomerByIdAsync(customerId);

        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }

        decimal interest;

        while (true)
        {
            string input = ConsoleInput.ReadText("Interest rate (0-100): ");

            if (decimal.TryParse(input, out interest) &&
                interest >= 0 && interest <= 100)
                break;

            Console.WriteLine("Enter an interest rate from 0 to 100.");
        }

        var loan = new Loan
        {
            CustomerId = customerId,
            LoanType = ConsoleInput.ReadText("Loan type: "),
            LoanAmount = ConsoleInput.ReadAmount("Loan amount: "),
            InterestRate = interest,
            LoanDate = DateTime.Now,
            Status = "Pending"
        };

        await _service.AddLoanAsync(loan);
        Console.WriteLine($"Loan added. ID: {loan.LoanId}");
    }

    private async Task ViewAsync()
    {
        var loans = await _service.GetAllLoansAsync();

        foreach (var loan in loans)
            Console.WriteLine($"{loan.LoanId} | Customer: {loan.CustomerId} | {loan.LoanType} | {loan.LoanAmount:F2} | {loan.InterestRate}% | {loan.Status}");

        if (loans.Count == 0)
            Console.WriteLine("No loans found.");
    }

    private async Task SearchAsync()
    {
        int id = ConsoleInput.ReadInt("Loan ID: ");
        var loan = await _service.GetLoanByIdAsync(id);

        if (loan == null)
        {
            Console.WriteLine("Loan not found.");
            return;
        }

        Console.WriteLine($"{loan.LoanId} | Customer: {loan.CustomerId} | {loan.LoanType} | {loan.LoanAmount:F2} | {loan.InterestRate}% | {loan.Status}");
    }

    private async Task UpdateAsync()
    {
        int id = ConsoleInput.ReadInt("Loan ID: ");
        var loan = await _service.GetLoanByIdAsync(id);

        if (loan == null)
        {
            Console.WriteLine("Loan not found.");
            return;
        }

        loan.LoanType = ConsoleInput.ReadText("New loan type: ");
        loan.LoanAmount = ConsoleInput.ReadAmount("New loan amount: ");

        string status = ConsoleInput.ReadText("Status (Pending/Approved/Rejected): ");

        if (status.Equals("Pending", StringComparison.OrdinalIgnoreCase))
            loan.Status = "Pending";
        else if (status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
            loan.Status = "Approved";
        else if (status.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
            loan.Status = "Rejected";
        else
        {
            Console.WriteLine("Invalid loan status.");
            return;
        }

        await _service.UpdateLoanAsync(loan);
        Console.WriteLine("Loan updated.");
    }

    private async Task DeleteAsync()
    {
        int id = ConsoleInput.ReadInt("Loan ID: ");
        await _service.DeleteLoanAsync(id);
        Console.WriteLine("Delete request completed.");
    }
}
