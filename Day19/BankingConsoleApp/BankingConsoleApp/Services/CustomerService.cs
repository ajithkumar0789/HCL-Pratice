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

    public void AddCustomer(Customer customer)
    {
        if (string.IsNullOrWhiteSpace(customer.FullName))
            throw new ArgumentException("Customer name is required.");

        _repository.Add(customer);
    }

    public Customer? GetCustomerById(int customerId)
    {
        if (customerId <= 0)
            throw new ArgumentException("Customer ID must be greater than zero.");

        return _repository.GetById(customerId);
    }

    public List<Customer> GetAllCustomers()
    {
        return _repository.GetAll();
    }

    public void UpdateCustomer(Customer customer)
    {
        if (customer.CustomerId <= 0)
            throw new ArgumentException("A valid customer ID is required.");

        if (string.IsNullOrWhiteSpace(customer.FullName))
            throw new ArgumentException("Customer name is required.");

        _repository.Update(customer);
    }

    public void DeleteCustomer(int customerId)
    {
        if (customerId <= 0)
            throw new ArgumentException("Customer ID must be greater than zero.");

        _repository.Delete(customerId);
    }
}