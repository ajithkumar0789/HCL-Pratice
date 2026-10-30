using BankingConsoleApp.Data;
using BankingConsoleApp.Model;
using Microsoft.EntityFrameworkCore;

namespace BankingConsoleApp.Repositories;

public class TransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task<Transaction?> GetByIdAsync(int transactionId)
    {
        return await _context.Transactions
            .AsNoTracking()
            .Include(t => t.BankAccount)
            .FirstOrDefaultAsync(t => t.TransactionId == transactionId);
    }

    public async Task<List<Transaction>> GetAllAsync()
    {
        return await _context.Transactions
            .AsNoTracking()
            .Include(t => t.BankAccount)
            .ToListAsync();
    }

    public async Task UpdateAsync(Transaction transaction)
    {
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int transactionId)
    {
        var transaction = await _context.Transactions.FindAsync(transactionId);

        if (transaction != null)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }
    }
}