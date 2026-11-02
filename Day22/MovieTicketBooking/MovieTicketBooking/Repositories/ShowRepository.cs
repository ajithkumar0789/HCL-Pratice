using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories;

public class ShowRepository : IShowRepository
{
    private readonly ApplicationDbContext _context;

    public ShowRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    private IQueryable<Show> ShowQuery()
    {
        return _context.Shows
            .Include(s => s.Movie)
            .Include(s => s.Screen)
                .ThenInclude(s => s!.Theatre);
    }

    public async Task<List<Show>> GetAllAsync(
        DateTime? date, int? movieId, int? theatreId)
    {
        var query = ShowQuery().AsNoTracking();

        if (date.HasValue)
            query = query.Where(s => s.StartTime.Date == date.Value.Date);

        if (movieId.HasValue)
            query = query.Where(s => s.MovieId == movieId.Value);

        if (theatreId.HasValue)
            query = query.Where(s => s.Screen!.TheatreId == theatreId.Value);

        return await query
            .OrderBy(s => s.StartTime)
            .ToListAsync();
    }

    public async Task<Show?> GetByIdAsync(int id)
    {
        return await ShowQuery()
            .FirstOrDefaultAsync(s => s.ShowId == id);
    }

    public async Task<List<Show>> GetByMovieIdAsync(int movieId)
    {
        return await ShowQuery()
            .AsNoTracking()
            .Where(s => s.MovieId == movieId)
            .OrderBy(s => s.StartTime)
            .ToListAsync();
    }

    public async Task<List<Show>> GetByTheatreIdAsync(int theatreId)
    {
        return await ShowQuery()
            .AsNoTracking()
            .Where(s => s.Screen!.TheatreId == theatreId)
            .OrderBy(s => s.StartTime)
            .ToListAsync();
    }

    public async Task<bool> MovieExistsAsync(int movieId)
    {
        return await _context.Movies
            .AnyAsync(m => m.MovieId == movieId);
    }

    public async Task<bool> ScreenExistsAsync(int screenId)
    {
        return await _context.Screens
            .AnyAsync(s => s.ScreenId == screenId);
    }

    public async Task<bool> HasOverlapAsync(
        int screenId,
        DateTime startTime,
        DateTime endTime,
        int? excludeShowId = null)
    {
        var query = _context.Shows.Where(s =>
            s.ScreenId == screenId &&
            startTime < s.EndTime &&
            endTime > s.StartTime);

        if (excludeShowId.HasValue)
            query = query.Where(s => s.ShowId != excludeShowId.Value);

        return await query.AnyAsync();
    }

    public async Task CreateWithSeatsAsync(
        Show show, List<ShowSeat> showSeats)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            _context.Shows.Add(show);
            await _context.SaveChangesAsync();

            foreach (var showSeat in showSeats)
                showSeat.ShowId = show.ShowId;

            _context.ShowSeats.AddRange(showSeats);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateAsync(Show show)
    {
        _context.Shows.Update(show);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var show = await _context.Shows.FindAsync(id);

        if (show == null)
            return false;

        _context.Shows.Remove(show);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> HasBookingsAsync(int showId)
    {
        return await _context.Bookings
            .AnyAsync(b => b.ShowId == showId);
    }

    public async Task<List<ShowSeat>> GetSeatsAsync(
        int showId, string? status)
    {
        var query = _context.ShowSeats
            .Include(ss => ss.Seat)
            .Where(ss => ss.ShowId == showId);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(ss => ss.Status == status);

        return await query
            .AsNoTracking()
            .OrderBy(ss => ss.Seat!.RowName)
            .ThenBy(ss => ss.Seat!.SeatNumber)
            .ToListAsync();
    }
}