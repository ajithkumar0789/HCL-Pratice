using BankingConsoleApp.Model;
using BankingConsoleApp.Repositories;

namespace BankingConsoleApp.Services;

public class CustomerService
{
    private readonly CustomerRepository _repository;

    public CustomerService(CustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task AddCustomerAsync(Customer customer)
    {
        if (string.IsNullOrWhiteSpace(customer.FullName))
            throw new ArgumentException("Customer name is required.");

        await _repository.AddAsync(customer);
    }

    public async Task<Customer?> GetCustomerByIdAsync(int customerId)
    {
        if (customerId <= 0)
            throw new ArgumentException("Customer ID must be greater than zero.");

        return await _repository.GetByIdAsync(customerId);
    }

    public async Task<List<Customer>> GetAllCustomersAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        if (customer.CustomerId <= 0)
            throw new ArgumentException("A valid customer ID is required.");

        if (string.IsNullOrWhiteSpace(customer.FullName))
            throw new ArgumentException("Customer name is required.");

        await _repository.UpdateAsync(customer);
    }

    public async Task DeleteCustomerAsync(int customerId)
    {
        if (customerId <= 0)
            throw new ArgumentException("Customer ID must be greater than zero.");

        await _repository.DeleteAsync(customerId);
    }
}