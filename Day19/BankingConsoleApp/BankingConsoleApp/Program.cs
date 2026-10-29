
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text.RegularExpressions;
using BankingConsoleApp.Data;
using BankingConsoleApp.Helper;
using BankingConsoleApp.Menus;
using BankingConsoleApp.Model;
using BankingConsoleApp.Repositories;
using BankingConsoleApp.Services;
using Microsoft.EntityFrameworkCore;

const string connectionString =
    "Server=(localdb)\\MSSQLLocalDB;Database=BankingConsoleDb;Trusted_Connection=True;TrustServerCertificate=True;";

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer(connectionString)
    .Options;

using var context = new AppDbContext(options);

var customerRepository = new CustomerRepository(context);
var accountRepository = new BankAccountRepository(context);

var customerService = new CustomerService(customerRepository);
var accountService = new BankAccountService(accountRepository);

var menu = new MainMenu();

while (true)
{
    try
    {
        Console.Clear();
        menu.Display();

        string? choice = Console.ReadLine()?.Trim();

        switch (choice)
        {
            case "1":
                AddCustomer();
                break;

            case "2":
                ViewCustomers();
                break;

            case "3":
                AddAccount();
                break;

            case "4":
                ViewAccounts();
                break;

            case "5":
                DepositMoney();
                break;

            case "6":
                WithdrawMoney();
                break;

            case "7":
                Console.WriteLine(
                    "Thank you for using Banking Console App!");
                return;

            default:
                Console.WriteLine(
                    "Invalid choice. Enter a number from 1 to 7.");
                break;
        }
    }
    catch (DbUpdateException)
    {
        Console.WriteLine(
            "Database error: Check for duplicate account numbers, " +
            "invalid customer IDs, or database connection problems.");
    }
    catch (Exception ex)
    {
        ErrorHandler.Handle(ex);
    }

    Console.WriteLine("\nPress Enter to continue...");
    Console.ReadLine();
}

void AddCustomer()
{
    Console.WriteLine("\n--- Add Customer ---");

    string name = ReadRequiredText("Enter customer name: ");

    string email;

    while (true)
    {
        email = ReadRequiredText("Enter email address: ");

        if (new EmailAddressAttribute().IsValid(email))
            break;

        Console.WriteLine("Invalid email. Example: user@example.com");
    }

    string phone;

    while (true)
    {
        phone = ReadRequiredText("Enter 10-digit phone number: ");

        if (Regex.IsMatch(phone, @"^\d{10}$"))
            break;

        Console.WriteLine(
            "Invalid phone number. Enter exactly 10 digits.");
    }

    var customer = new Customer
    {
        FullName = name,
        Email = email,
        Phone = phone
    };

    customerService.AddCustomer(customer);

    Console.WriteLine(
        $"Customer added successfully! Customer ID: {customer.CustomerId}");
}

void ViewCustomers()
{
    Console.WriteLine("\n--- All Customers ---");

    var customers = customerService.GetAllCustomers();

    if (customers.Count == 0)
    {
        Console.WriteLine("No customers found.");
        return;
    }

    foreach (var customer in customers)
    {
        Console.WriteLine(
            $"ID: {customer.CustomerId} | " +
            $"Name: {customer.FullName} | " +
            $"Email: {customer.Email} | " +
            $"Phone: {customer.Phone}");
    }
}

void AddAccount()
{
    Console.WriteLine("\n--- Add Bank Account ---");

    int customerId = ReadPositiveInt("Enter customer ID: ");

    var customer = customerService.GetCustomerById(customerId);

    if (customer == null)
    {
        Console.WriteLine("Customer not found. Add the customer first.");
        return;
    }

    string accountNumber;

    while (true)
    {
        accountNumber = ReadRequiredText(
            "Enter account number (10-16 digits): ");

        if (!Regex.IsMatch(accountNumber, @"^\d{10,16}$"))
        {
            Console.WriteLine(
                "Account number must contain 10 to 16 digits.");
            continue;
        }

        if (accountService.GetAllAccounts()
            .Any(a => a.AccountNumber == accountNumber))
        {
            Console.WriteLine(
                "This account number already exists. Try another.");
            continue;
        }

        break;
    }

    var account = new BankAccount
    {
        AccountHolder = customer.FullName,
        AccountNumber = accountNumber,
        CustomerId = customerId
    };

    accountService.AddAccount(account);

    Console.WriteLine(
        $"Account created successfully! Account ID: {account.BankAccountId}");
}

void ViewAccounts()
{
    Console.WriteLine("\n--- All Bank Accounts ---");

    var accounts = accountService.GetAllAccounts();

    if (accounts.Count == 0)
    {
        Console.WriteLine("No bank accounts found.");
        return;
    }

    foreach (var account in accounts)
    {
        Console.WriteLine(
            $"ID: {account.BankAccountId} | " +
            $"Number: {account.AccountNumber} | " +
            $"Holder: {account.Customer?.FullName ?? account.AccountHolder} | " +
            $"Balance: {account.Balance:F2}");
    }
}

void DepositMoney()
{
    Console.WriteLine("\n--- Deposit Money ---");

    int accountId = ReadPositiveInt("Enter account ID: ");
    decimal amount = ReadPositiveAmount("Enter deposit amount: ");

    accountService.Deposit(accountId, amount);

    Console.WriteLine("Deposit successful!");
}

void WithdrawMoney()
{
    Console.WriteLine("\n--- Withdraw Money ---");

    int accountId = ReadPositiveInt("Enter account ID: ");
    decimal amount = ReadPositiveAmount("Enter withdrawal amount: ");

    accountService.Withdraw(accountId, amount);

    Console.WriteLine("Withdrawal successful!");
}

string ReadRequiredText(string prompt)
{
    while (true)
    {
        Console.Write(prompt);

        string? value = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(value))
            return value.Trim();

        Console.WriteLine("This field is required. Please try again.");
    }
}

int ReadPositiveInt(string prompt)
{
    while (true)
    {
        string input = ReadRequiredText(prompt);

        if (int.TryParse(input, out int value) && value > 0)
            return value;

        Console.WriteLine("Enter a valid positive whole number.");
    }
}

decimal ReadPositiveAmount(string prompt)
{
    while (true)
    {
        string input = ReadRequiredText(prompt);

        if (decimal.TryParse(input, out decimal amount) &&
            amount > 0 &&
            amount <= 9999999999999999.99m &&
            decimal.Round(amount, 2) == amount)
        {
            return amount;
        }

        Console.WriteLine(
            "Enter a valid positive amount with at most 2 decimal places.");
    }
}
