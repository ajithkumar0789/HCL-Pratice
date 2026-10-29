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

    public void Add(BankAccount account)
    {
        _context.BankAccounts.Add(account);
        _context.SaveChanges();
    }

    public BankAccount? GetById(int accountId)
    {
        return _context.BankAccounts
            .Include(a => a.Customer)
            .FirstOrDefault(a => a.BankAccountId == accountId);
    }

    public List<BankAccount> GetAll()
    {
        return _context.BankAccounts
            .Include(a => a.Customer)
            .ToList();
    }

    public void Update(BankAccount account)
    {
        _context.BankAccounts.Update(account);
        _context.SaveChanges();
    }

    public void Delete(int accountId)
    {
        var account = _context.BankAccounts.Find(accountId);

        if (account != null)
        {
            _context.BankAccounts.Remove(account);
            _context.SaveChanges();
        }
    }
}