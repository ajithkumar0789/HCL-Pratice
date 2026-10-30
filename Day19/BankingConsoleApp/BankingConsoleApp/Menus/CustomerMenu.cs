
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using BankingConsoleApp.Model;
using BankingConsoleApp.Services;

namespace BankingConsoleApp.Menus;

public class CustomerMenu
{
    private readonly CustomerService _service;

    public CustomerMenu(CustomerService service)
    {
        _service = service;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== CUSTOMER MANAGEMENT =====");
            Console.WriteLine("1. Add Customer");
            Console.WriteLine("2. View Customers");
            Console.WriteLine("3. Search Customer");
            Console.WriteLine("4. Update Customer");
            Console.WriteLine("5. Delete Customer");
            Console.WriteLine("6. Back");

            switch (ConsoleInput.ReadText("Enter choice: "))
            {
                case "1":
                    await AddAsync();
                    break;
                case "2":
                    await ViewAsync();
                    break;
                case "3":
                    await SearchAsync();
                    break;
                case "4":
                    await UpdateAsync();
                    break;
                case "5":
                    await DeleteAsync();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }

            ConsoleInput.Pause();
        }
    }

    private async Task AddAsync()
    {
        string name = ConsoleInput.ReadText("Customer name: ");

        string email;
        while (true)
        {
            email = ConsoleInput.ReadText("Email: ");
            if (new EmailAddressAttribute().IsValid(email))
                break;
            Console.WriteLine("Invalid email.");
        }

        string phone;
        while (true)
        {
            phone = ConsoleInput.ReadText("10-digit phone: ");
            if (Regex.IsMatch(phone, @"^\d{10}$"))
                break;
            Console.WriteLine("Enter exactly 10 digits.");
        }

        var customer = new Customer
        {
            FullName = name,
            Email = email,
            Phone = phone
        };

        await _service.AddCustomerAsync(customer);
        Console.WriteLine($"Customer added. ID: {customer.CustomerId}");
    }

    private async Task ViewAsync()
    {
        var customers = await _service.GetAllCustomersAsync();

        foreach (var customer in customers)
        {
            Console.WriteLine(
                $"{customer.CustomerId} | {customer.FullName} | " +
                $"{customer.Email} | {customer.Phone}");
        }

        if (customers.Count == 0)
            Console.WriteLine("No customers found.");
    }

    private async Task SearchAsync()
    {
        int id = ConsoleInput.ReadInt("Customer ID: ");
        var customer = await _service.GetCustomerByIdAsync(id);

        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }

        Console.WriteLine(
            $"{customer.CustomerId} | {customer.FullName} | " +
            $"{customer.Email} | {customer.Phone}");
    }

    private async Task UpdateAsync()
    {
        int id = ConsoleInput.ReadInt("Customer ID: ");
        var customer = await _service.GetCustomerByIdAsync(id);

        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }

        customer.FullName = ConsoleInput.ReadText("New name: ");
        customer.Email = ConsoleInput.ReadText("New email: ");
        customer.Phone = ConsoleInput.ReadText("New phone: ");

        await _service.UpdateCustomerAsync(customer);
        Console.WriteLine("Customer updated.");
    }

    private async Task DeleteAsync()
    {
        int id = ConsoleInput.ReadInt("Customer ID: ");
        await _service.DeleteCustomerAsync(id);
        Console.WriteLine("Delete request completed.");
    }
}
