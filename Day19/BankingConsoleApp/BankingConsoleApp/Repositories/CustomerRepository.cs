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

    public void Add(Customer customer)
    {
        _context.Customers.Add(customer);
        _context.SaveChanges();
    }

    public Customer? GetById(int customerId)
    {
        return _context.Customers
            .Include(c => c.BankAccounts)
            .FirstOrDefault(c => c.CustomerId == customerId);
    }

    public List<Customer> GetAll()
    {
        return _context.Customers
            .Include(c => c.BankAccounts)
            .ToList();
    }

    public void Update(Customer customer)
    {
        _context.Customers.Update(customer);
        _context.SaveChanges();
    }

    public void Delete(int customerId)
    {
        var customer = _context.Customers.Find(customerId);

        if (customer != null)
        {
            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }
    }
}