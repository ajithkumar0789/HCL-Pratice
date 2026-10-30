using BankingConsoleApp.Data;
using BankingConsoleApp.Model;
using Microsoft.EntityFrameworkCore;

namespace BankingConsoleApp.Repositories;

public class LoanRepository
{
    private readonly AppDbContext _context;

    public LoanRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Loan loan)
    {
        await _context.Loans.AddAsync(loan);
        await _context.SaveChangesAsync();
    }

    public async Task<Loan?> GetByIdAsync(int loanId)
    {
        return await _context.Loans
            .AsNoTracking()
            .Include(l => l.Customer)
            .FirstOrDefaultAsync(l => l.LoanId == loanId);
    }

    public async Task<List<Loan>> GetAllAsync()
    {
        return await _context.Loans
            .AsNoTracking()
            .Include(l => l.Customer)
            .ToListAsync();
    }

    public async Task UpdateAsync(Loan loan)
    {
        var existingLoan = await _context.Loans
            .FirstOrDefaultAsync(l => l.LoanId == loan.LoanId);

        if (existingLoan == null)
        {
            throw new Exception("Loan not found.");
        }

        // Update the existing tracked object
        existingLoan.LoanType = loan.LoanType;
        existingLoan.LoanAmount = loan.LoanAmount;
        existingLoan.InterestRate = loan.InterestRate;
        existingLoan.LoanDate = loan.LoanDate;
        existingLoan.Status = loan.Status;
        existingLoan.CustomerId = loan.CustomerId;

        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(int loanId)
    {
        var loan = await _context.Loans.FindAsync(loanId);

        if (loan != null)
        {
            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();
        }
    }
}