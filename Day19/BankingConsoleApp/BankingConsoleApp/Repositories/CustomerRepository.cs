using BankingConsoleApp.Data;
using BankingConsoleApp.Model;
using Microsoft.EntityFrameworkCore;

namespace BankingConsoleApp.Repositories;

public class CustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }

    public async Task<Customer?> GetByIdAsync(int customerId)
    {
        return await _context.Customers
            .AsNoTracking()
            .Include(c => c.BankAccounts)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _context.Customers
            .AsNoTracking()
            .Include(c => c.BankAccounts)
            .ToListAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int customerId)
    {
        var customer = await _context.Customers.FindAsync(customerId);

        if (customer != null)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }
}