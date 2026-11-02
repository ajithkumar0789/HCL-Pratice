using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories;

public class ScreenRepository : IScreenRepository
{
    private readonly ApplicationDbContext _context;

    public ScreenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Screen>> GetAllAsync()
    {
        return await _context.Screens
            .Include(s => s.Theatre)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Screen?> GetByIdAsync(int id)
    {
        return await _context.Screens
            .Include(s => s.Theatre)
            .FirstOrDefaultAsync(s => s.ScreenId == id);
    }

    public async Task<List<Screen>> GetByTheatreIdAsync(int theatreId)
    {
        return await _context.Screens
            .Include(s => s.Theatre)
            .Where(s => s.TheatreId == theatreId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(Screen screen)
    {
        _context.Screens.Add(screen);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Screen screen)
    {
        _context.Screens.Update(screen);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var screen = await _context.Screens.FindAsync(id);

        if (screen == null)
            return false;

        _context.Screens.Remove(screen);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> HasSeatsAsync(int id)
    {
        return await _context.Seats
            .AnyAsync(s => s.ScreenId == id);
    }

    public async Task<bool> HasShowsAsync(int id)
    {
        return await _context.Shows
            .AnyAsync(s => s.ScreenId == id);
    }

    public async Task<bool> TheatreExistsAsync(int theatreId)
    {
        return await _context.Theatres
            .AnyAsync(t => t.TheatreId == theatreId);
    }
}