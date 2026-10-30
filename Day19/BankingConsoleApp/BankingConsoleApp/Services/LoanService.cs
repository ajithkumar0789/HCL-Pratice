
using BankingConsoleApp.Model;
using BankingConsoleApp.Repositories;

namespace BankingConsoleApp.Services;

public class LoanService
{
    private readonly LoanRepository _repository;
    private readonly AuditLogService _auditLogService;

    public LoanService(
        LoanRepository repository,
        AuditLogService auditLogService)
    {
        _repository = repository;
        _auditLogService = auditLogService;
    }

    public async Task AddLoanAsync(Loan loan)
    {
        if (loan.CustomerId <= 0)
            throw new ArgumentException("A valid customer ID is required.");

        if (string.IsNullOrWhiteSpace(loan.LoanType))
            throw new ArgumentException("Loan type is required.");

        if (loan.LoanAmount <= 0)
            throw new ArgumentException("Loan amount must be greater than zero.");

        if (loan.InterestRate < 0 || loan.InterestRate > 100)
            throw new ArgumentException("Interest rate must be between 0 and 100.");

        await _repository.AddAsync(loan);

        await _auditLogService.LogAsync(
            "CREATE",
            "Loan",
            loan.LoanId,
            $"New {loan.LoanType} loan created. Amount: {loan.LoanAmount}");
    }

    public async Task<Loan?> GetLoanByIdAsync(int loanId)
    {
        if (loanId <= 0)
            throw new ArgumentException("Loan ID must be greater than zero.");

        return await _repository.GetByIdAsync(loanId);
    }

    public async Task<List<Loan>> GetAllLoansAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task UpdateLoanAsync(Loan loan)
    {
        if (loan.LoanId <= 0)
            throw new ArgumentException("A valid loan ID is required.");

        if (string.IsNullOrWhiteSpace(loan.LoanType))
            throw new ArgumentException("Loan type is required.");

        if (loan.LoanAmount <= 0)
            throw new ArgumentException("Loan amount must be greater than zero.");

        if (loan.InterestRate < 0 || loan.InterestRate > 100)
            throw new ArgumentException("Interest rate must be between 0 and 100.");

        if (loan.Status != "Pending" &&
            loan.Status != "Approved" &&
            loan.Status != "Rejected")
            throw new ArgumentException("Invalid loan status.");

        await _repository.UpdateAsync(loan);

        await _auditLogService.LogAsync(
            "UPDATE",
            "Loan",
            loan.LoanId,
            $"Loan updated. Status: {loan.Status}, Amount: {loan.LoanAmount}");
    }

    public async Task DeleteLoanAsync(int loanId)
    {
        if (loanId <= 0)
            throw new ArgumentException("Loan ID must be greater than zero.");

        var loan = await _repository.GetByIdAsync(loanId);

        if (loan == null)
            throw new ArgumentException("Loan not found.");

        await _repository.DeleteAsync(loanId);

        await _auditLogService.LogAsync(
            "DELETE",
            "Loan",
            loanId,
            $"Loan deleted. Type: {loan.LoanType}, Amount: {loan.LoanAmount}");
    }
}
