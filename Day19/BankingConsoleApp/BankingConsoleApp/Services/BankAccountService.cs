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

    public async Task AddAccountAsync(BankAccount account)
    {
        if (string.IsNullOrWhiteSpace(account.AccountNumber))
            throw new ArgumentException("Account number is required.");

        if (account.CustomerId <= 0)
            throw new ArgumentException("A valid customer ID is required.");

        await _repository.AddAsync(account);
    }

    public async Task<BankAccount?> GetAccountByIdAsync(int accountId)
    {
        if (accountId <= 0)
            throw new ArgumentException("Account ID must be greater than zero.");

        return await _repository.GetByIdAsync(accountId);
    }

    public async Task<List<BankAccount>> GetAllAccountsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task DepositAsync(int accountId, decimal amount)
    {
        var account = await GetAccountByIdAsync(accountId)
            ?? throw new InvalidOperationException("Account not found.");

        if (!account.Deposit(amount))
            throw new ArgumentException("Deposit amount must be greater than zero.");

        await _repository.UpdateAsync(account);
    }

    public async Task WithdrawAsync(int accountId, decimal amount)
    {
        var account = await GetAccountByIdAsync(accountId)
            ?? throw new InvalidOperationException("Account not found.");

        if (!account.Withdraw(amount))
            throw new ArgumentException(
                "Invalid amount or insufficient balance.");

        await _repository.UpdateAsync(account);
    }

    public async Task DeleteAccountAsync(int accountId)
    {
        if (accountId <= 0)
            throw new ArgumentException("Account ID must be greater than zero.");

        await _repository.DeleteAsync(accountId);
    }
}