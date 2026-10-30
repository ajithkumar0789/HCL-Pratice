namespace BankingConsoleApp.Model;

public class AuditLog
{
    public int AuditLogId { get; set; }

    public string Action { get; set; } = string.Empty;

    public string EntityName { get; set; } = string.Empty;

    public int RecordId { get; set; }

    public string Description { get; set; } = string.Empty;

    public string PerformedBy { get; set; } = "System";

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}