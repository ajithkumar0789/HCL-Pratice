using BankingConsoleApp.Model;
using BankingConsoleApp.Repositories;

namespace BankingConsoleApp.Services;

public class BankAccountService
{
    private readonly BankAccountRepository _repository;

    public BankAccountService(BankAccountRepository repository)
    {
        _repository = repository;
    }

    public void AddAccount(BankAccount account)
    {
        if (string.IsNullOrWhiteSpace(account.AccountNumber))
            throw new ArgumentException("Account number is required.");

        if (account.CustomerId <= 0)
            throw new ArgumentException("A valid customer ID is required.");

        _repository.Add(account);
    }

    public BankAccount? GetAccountById(int accountId)
    {
        if (accountId <= 0)
            throw new ArgumentException("Account ID must be greater than zero.");

        return _repository.GetById(accountId);
    }

    public List<BankAccount> GetAllAccounts()
    {
        return _repository.GetAll();
    }

    public void Deposit(int accountId, decimal amount)
    {
        var account = GetAccountById(accountId)
            ?? throw new InvalidOperationException("Account not found.");

        if (!account.Deposit(amount))
            throw new ArgumentException("Deposit amount must be greater than zero.");

        _repository.Update(account);
    }

    public void Withdraw(int accountId, decimal amount)
    {
        var account = GetAccountById(accountId)
            ?? throw new InvalidOperationException("Account not found.");

        if (!account.Withdraw(amount))
            throw new ArgumentException(
                "Invalid amount or insufficient balance.");

        _repository.Update(account);
    }

    public void DeleteAccount(int accountId)
    {
        if (accountId <= 0)
            throw new ArgumentException("Account ID must be greater than zero.");

        _repository.Delete(accountId);
    }
}