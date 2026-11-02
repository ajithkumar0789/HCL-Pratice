using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories;

public class SeatRepository : ISeatRepository
{
    private readonly ApplicationDbContext _context;

    public SeatRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Seat>> GetByScreenIdAsync(int screenId)
    {
        return await _context.Seats
            .Where(s => s.ScreenId == screenId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Seat?> GetByIdAsync(int id)
    {
        return await _context.Seats
            .FirstOrDefaultAsync(s => s.SeatId == id);
    }

    public async Task AddAsync(Seat seat)
    {
        _context.Seats.Add(seat);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(List<Seat> seats)
    {
        _context.Seats.AddRange(seats);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Seat seat)
    {
        _context.Seats.Update(seat);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var seat = await _context.Seats.FindAsync(id);

        if (seat == null)
            return false;

        _context.Seats.Remove(seat);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ScreenExistsAsync(int screenId)
    {
        return await _context.Screens
            .AnyAsync(s => s.ScreenId == screenId);
    }

    public async Task<bool> HasHistoryAsync(int seatId)
    {
        return await _context.ShowSeats
            .AnyAsync(s => s.SeatId == seatId)
            ||
            await _context.BookingSeats
                .AnyAsync(b => b.SeatId == seatId);
    }
}