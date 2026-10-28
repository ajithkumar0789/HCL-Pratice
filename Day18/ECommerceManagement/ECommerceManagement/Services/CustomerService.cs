using ECommerceManagement.Models;
using ECommerceManagement.Repositories;

namespace ECommerceManagement.Services;

public class CustomerService
{
    private readonly CustomerRepository repository = new();

    public List<Customer> GetAll() => repository.GetAll();

    public void Add(Customer customer)
    {
        Validate(customer);
        repository.Add(customer);
    }

    public bool Update(Customer customer)
    {
        if (customer.CustomerId <= 0)
            throw new ArgumentException("Enter a valid customer ID.");

        Validate(customer);
        return repository.Update(customer);
    }

    public bool Deactivate(int id) => repository.Deactivate(id);

    private static void Validate(Customer customer)
    {
        if (string.IsNullOrWhiteSpace(customer.CustomerName))
            throw new ArgumentException("Customer name is required.");

        if (string.IsNullOrWhiteSpace(customer.Email) ||
            !customer.Email.Contains('@'))
            throw new ArgumentException("Enter a valid email address.");

        if (string.IsNullOrWhiteSpace(customer.PhoneNumber) ||
            customer.PhoneNumber.Length != 10 ||
            !customer.PhoneNumber.All(char.IsDigit))
            throw new ArgumentException(
                "Phone number must contain exactly 10 digits.");
    }
}