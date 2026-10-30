using BankingConsoleApp.Services;

namespace BankingConsoleApp.Menus;

public class AuditLogMenu
{
    private readonly AuditLogService _service;

    public AuditLogMenu(AuditLogService service)
    {
        _service = service;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("===== AUDIT LOG MANAGEMENT =====");
            Console.WriteLine("1. View All Audit Logs");
            Console.WriteLine("2. Back");
            Console.Write("Enter choice: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ViewLogsAsync();
                    break;

                case "2":
                    return;

                default:
                    Console.WriteLine("Invalid choice.");
                    Console.ReadLine();
                    break;
            }
        }
    }

    private async Task ViewLogsAsync()
    {
        var logs = await _service.GetAllLogsAsync();

        Console.WriteLine("\n===== AUDIT HISTORY =====");

        if (logs.Count == 0)
        {
            Console.WriteLine("No audit records found.");
        }

        foreach (var log in logs)
        {
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"ID: {log.AuditLogId}");
            Console.WriteLine($"Action: {log.Action}");
            Console.WriteLine($"Entity: {log.EntityName}");
            Console.WriteLine($"Record ID: {log.RecordId}");
            Console.WriteLine($"Description: {log.Description}");
            Console.WriteLine($"Performed By: {log.PerformedBy}");
            Console.WriteLine($"Date: {log.CreatedAt}");
        }

        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }
}