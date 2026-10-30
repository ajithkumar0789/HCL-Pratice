using BankingConsoleApp.Data;
using BankingConsoleApp.Model;
using Microsoft.EntityFrameworkCore;

namespace BankingConsoleApp.Repositories;

public class AuditLogRepository
{
    private readonly AppDbContext _context;

    public AuditLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AuditLog auditLog)
    {
        await _context.AuditLogs.AddAsync(auditLog);
        await _context.SaveChangesAsync();
    }

    public async Task<List<AuditLog>> GetAllAsync()
    {
        return await _context.AuditLogs
            .AsNoTracking()
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }
}