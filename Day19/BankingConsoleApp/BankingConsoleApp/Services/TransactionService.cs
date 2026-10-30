using BankingConsoleApp.Model;
using BankingConsoleApp.Repositories;

namespace BankingConsoleApp.Services;

public class TransactionService
{
    private readonly TransactionRepository _repository;

    public TransactionService(TransactionRepository repository)
    {
        _repository = repository;
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        if (transaction.BankAccountId <= 0)
            throw new ArgumentException("A valid bank account ID is required.");

        if (transaction.Amount <= 0)
            throw new ArgumentException("Transaction amount must be greater than zero.");

        if (transaction.TransactionType != "Deposit" &&
            transaction.TransactionType != "Withdrawal")
        {
            throw new ArgumentException("Invalid transaction type.");
        }

        await _repository.AddAsync(transaction);
    }

    public async Task<Transaction?> GetTransactionByIdAsync(int transactionId)
    {
        if (transactionId <= 0)
            throw new ArgumentException("Transaction ID must be greater than zero.");

        return await _repository.GetByIdAsync(transactionId);
    }

    public async Task<List<Transaction>> GetAllTransactionsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task UpdateTransactionAsync(Transaction transaction)
    {
        if (transaction.TransactionId <= 0)
            throw new ArgumentException("A valid transaction ID is required.");

        if (transaction.Amount <= 0)
            throw new ArgumentException("Transaction amount must be greater than zero.");

        await _repository.UpdateAsync(transaction);
    }

    public async Task DeleteTransactionAsync(int transactionId)
    {
        if (transactionId <= 0)
            throw new ArgumentException("Transaction ID must be greater than zero.");

        await _repository.DeleteAsync(transactionId);
    }
}
