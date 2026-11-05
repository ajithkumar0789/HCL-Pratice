
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories;

public class CollegeRepository : ICollegeRepository
{
    private readonly ApplicationDbContext _context;

    public CollegeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<College>> GetAllAsync()
    {
        return await _context.Colleges
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<College?> GetByIdAsync(int id)
    {
        return await _context.Colleges.FindAsync(id);
    }

    public async Task AddAsync(College college)
    {
        await _context.Colleges.AddAsync(college);
    }

    public void Update(College college)
    {
        _context.Colleges.Update(college);
    }

    public void Delete(College college)
    {
        _context.Colleges.Remove(college);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}