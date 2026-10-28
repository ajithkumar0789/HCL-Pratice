
namespace ECommerceManagement.Models;

public class AuditLog
{
    public int AuditId { get; set; }

    public string EntityName { get; set; } = "";

    public int EntityId { get; set; }

    public string Action { get; set; } = "";

    public string? Details { get; set; }

    public DateTime CreatedAt { get; set; }
}
