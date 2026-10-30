
using BankingConsoleApp.Data;
using BankingConsoleApp.Model;
using Microsoft.EntityFrameworkCore;

namespace BankingConsoleApp.Repositories;

public class BranchRepository
{
    private readonly AppDbContext _context;

    public BranchRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Branch branch)
    {
        await _context.Branches.AddAsync(branch);
        await _context.SaveChangesAsync();
    }

    public async Task<Branch?> GetByIdAsync(int id)
    {
        return await _context.Branches
            .Include(b => b.Employees)
            .Include(b => b.BankAccounts)
            .FirstOrDefaultAsync(b => b.BranchId == id);
    }

    public async Task<List<Branch>> GetAllAsync()
    {
        return await _context.Branches
            .AsNoTracking()
            .Include(b => b.Employees)
            .Include(b => b.BankAccounts)
            .ToListAsync();
    }

    public async Task UpdateAsync(Branch branch)
    {
        _context.Branches.Update(branch);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var branch = await _context.Branches.FindAsync(id);

        if (branch != null)
        {
            _context.Branches.Remove(branch);
            await _context.SaveChangesAsync();
        }
    }
}
