
using BankingConsoleApp.Model;
using BankingConsoleApp.Services;

namespace BankingConsoleApp.Menus;

public class BranchMenu
{
    private readonly BranchService _service;

    public BranchMenu(BranchService service)
    {
        _service = service;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== BRANCH MANAGEMENT =====");
            Console.WriteLine("1. Add Branch");
            Console.WriteLine("2. View Branches");
            Console.WriteLine("3. Search Branch");
            Console.WriteLine("4. Update Branch");
            Console.WriteLine("5. Delete Branch");
            Console.WriteLine("6. Back");

            switch (ConsoleInput.ReadText("Enter choice: "))
            {
                case "1": await AddAsync(); break;
                case "2": await ViewAsync(); break;
                case "3": await SearchAsync(); break;
                case "4": await UpdateAsync(); break;
                case "5": await DeleteAsync(); break;
                case "6": return;
                default: Console.WriteLine("Invalid choice."); break;
            }

            ConsoleInput.Pause();
        }
    }

    private async Task AddAsync()
    {
        var branch = new Branch
        {
            BranchName = ConsoleInput.ReadText("Branch name: "),
            BranchCode = ConsoleInput.ReadText("Branch code: "),
            Address = ConsoleInput.ReadText("Address: ")
        };

        await _service.AddBranchAsync(branch);
        Console.WriteLine($"Branch added. ID: {branch.BranchId}");
    }

    private async Task ViewAsync()
    {
        var branches = await _service.GetAllBranchesAsync();

        foreach (var branch in branches)
            Console.WriteLine($"{branch.BranchId} | {branch.BranchName} | {branch.BranchCode} | {branch.Address}");

        if (branches.Count == 0)
            Console.WriteLine("No branches found.");
    }

    private async Task SearchAsync()
    {
        int id = ConsoleInput.ReadInt("Branch ID: ");
        var branch = await _service.GetBranchByIdAsync(id);

        if (branch == null)
        {
            Console.WriteLine("Branch not found.");
            return;
        }

        Console.WriteLine($"{branch.BranchId} | {branch.BranchName} | {branch.BranchCode} | {branch.Address}");
    }

    private async Task UpdateAsync()
    {
        int id = ConsoleInput.ReadInt("Branch ID: ");
        var branch = await _service.GetBranchByIdAsync(id);

        if (branch == null)
        {
            Console.WriteLine("Branch not found.");
            return;
        }

        branch.BranchName = ConsoleInput.ReadText("New branch name: ");
        branch.BranchCode = ConsoleInput.ReadText("New branch code: ");
        branch.Address = ConsoleInput.ReadText("New address: ");

        await _service.UpdateBranchAsync(branch);
        Console.WriteLine("Branch updated.");
    }

    private async Task DeleteAsync()
    {
        int id = ConsoleInput.ReadInt("Branch ID: ");
        await _service.DeleteBranchAsync(id);
        Console.WriteLine("Delete request completed.");
    }
}
