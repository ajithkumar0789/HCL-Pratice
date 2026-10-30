using BankingConsoleApp.Model;
using BankingConsoleApp.Repositories;

namespace BankingConsoleApp.Services;

public class BranchService
{
    private readonly BranchRepository _repository;

    public BranchService(BranchRepository repository)
    {
        _repository = repository;
    }

    public async Task AddBranchAsync(Branch branch)
    {
        if (string.IsNullOrWhiteSpace(branch.BranchName))
            throw new ArgumentException("Branch name is required.");

        if (string.IsNullOrWhiteSpace(branch.BranchCode))
            throw new ArgumentException("Branch code is required.");

        if (string.IsNullOrWhiteSpace(branch.Address))
            throw new ArgumentException("Branch address is required.");

        await _repository.AddAsync(branch);
    }

    public async Task<Branch?> GetBranchByIdAsync(int branchId)
    {
        if (branchId <= 0)
            throw new ArgumentException("Branch ID must be greater than zero.");

        return await _repository.GetByIdAsync(branchId);
    }

    public async Task<List<Branch>> GetAllBranchesAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task UpdateBranchAsync(Branch branch)
    {
        if (branch.BranchId <= 0)
            throw new ArgumentException("A valid branch ID is required.");

        if (string.IsNullOrWhiteSpace(branch.BranchName))
            throw new ArgumentException("Branch name is required.");

        await _repository.UpdateAsync(branch);
    }

    public async Task DeleteBranchAsync(int branchId)
    {
        if (branchId <= 0)
            throw new ArgumentException("Branch ID must be greater than zero.");

        await _repository.DeleteAsync(branchId);
    }
}