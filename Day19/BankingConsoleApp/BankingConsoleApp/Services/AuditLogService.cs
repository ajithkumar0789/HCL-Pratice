using BankingConsoleApp.Model;
using BankingConsoleApp.Repositories;

namespace BankingConsoleApp.Services;

public class AuditLogService
{
    private readonly AuditLogRepository _repository;

    public AuditLogService(AuditLogRepository repository)
    {
        _repository = repository;
    }

    public async Task LogAsync(
        string action,
        string entityName,
        int recordId,
        string description,
        string performedBy = "Admin")
    {
        if (string.IsNullOrWhiteSpace(action))
            throw new ArgumentException("Action is required.");

        if (string.IsNullOrWhiteSpace(entityName))
            throw new ArgumentException("Entity name is required.");

        var auditLog = new AuditLog
        {
            Action = action,
            EntityName = entityName,
            RecordId = recordId,
            Description = description,
            PerformedBy = performedBy,
            CreatedAt = DateTime.Now
        };

        await _repository.AddAsync(auditLog);
    }

    public async Task<List<AuditLog>> GetAllLogsAsync()
    {
        return await _repository.GetAllAsync();
    }
}