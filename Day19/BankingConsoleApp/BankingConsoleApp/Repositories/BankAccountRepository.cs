using BankingConsoleApp.Data;
using BankingConsoleApp.Model;
using Microsoft.EntityFrameworkCore;

namespace BankingConsoleApp.Repositories;

public class BankAccountRepository
{
    private readonly AppDbContext _context;

    public BankAccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(BankAccount account)
    {
        await _context.BankAccounts.AddAsync(account);
        await _context.SaveChangesAsync();
    }

    public async Task<BankAccount?> GetByIdAsync(int accountId)
    {
        return await _context.BankAccounts
            .Include(a => a.Customer)
            .FirstOrDefaultAsync(a => a.BankAccountId == accountId);
    }

    public async Task<List<BankAccount>> GetAllAsync()
    {
        return await _context.BankAccounts
            .AsNoTracking()
            .Include(a => a.Customer)
            .ToListAsync();
    }

    public async Task UpdateAsync(BankAccount account)
    {
        _context.BankAccounts.Update(account);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int accountId)
    {
        var account = await _context.BankAccounts.FindAsync(accountId);

        if (account != null)
        {
            _context.BankAccounts.Remove(account);
            await _context.SaveChangesAsync();
        }
    }
}