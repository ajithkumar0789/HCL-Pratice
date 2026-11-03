using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories;

public class TheatreRepository : ITheatreRepository
{
    private readonly ApplicationDbContext _context;

    public TheatreRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Theatre>> GetAllAsync()
    {
        return await _context.Theatres
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Theatre?> GetByIdAsync(int id)
    {
        return await _context.Theatres
            .FirstOrDefaultAsync(t => t.TheatreId == id);
    }

    public async Task AddAsync(Theatre theatre)
    {
        _context.Theatres.Add(theatre);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Theatre theatre)
    {
        _context.Theatres.Update(theatre);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var theatre = await _context.Theatres.FindAsync(id);

        if (theatre == null)
            return false;

        _context.Theatres.Remove(theatre);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> HasScreensAsync(int id)
    {
        return await _context.Screens
            .AnyAsync(s => s.TheatreId == id);
    }
}