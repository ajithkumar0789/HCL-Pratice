
using System.Data;
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _context;

    public BookingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get show details along with its seats
    public async Task<Show?> GetShowWithSeatsAsync(int showId)
    {
        return await _context.Shows
            .Include(s => s.ShowSeats)
                .ThenInclude(ss => ss.Seat)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.ShowId == showId);
    }

    // Get booking using BookingId
    public async Task<Booking?> GetBookingByIdAsync(int bookingId)
    {
        return await _context.Bookings
            .Include(b => b.BookingSeats)
                .ThenInclude(bs => bs.Seat)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);
    }

    // Get booking using BookingReference
    public async Task<Booking?> GetBookingByReferenceAsync(
        string bookingReference)
    {
        return await _context.Bookings
            .Include(b => b.BookingSeats)
                .ThenInclude(bs => bs.Seat)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                b => b.BookingReference == bookingReference);
    }

    // Save booking and update seat statuses together
    public async Task AddBookingAsync(Booking booking)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync(
                IsolationLevel.Serializable);

        try
        {
            var seatIds = booking.BookingSeats
                .Select(bs => bs.SeatId)
                .ToList();

            if (seatIds.Count == 0 ||
                seatIds.Distinct().Count() != seatIds.Count)
            {
                throw new InvalidOperationException(
                    "Invalid or duplicate seat selection.");
            }

            var showSeats = await _context.ShowSeats
                .Where(ss =>
                    ss.ShowId == booking.ShowId &&
                    seatIds.Contains(ss.SeatId))
                .ToListAsync();

            if (showSeats.Count != seatIds.Count ||
                showSeats.Any(ss =>
                    ss.Status != ShowSeatStatuses.Available))
            {
                throw new InvalidOperationException(
                    "One or more selected seats are unavailable.");
            }

            // Mark selected seats as booked
            foreach (var showSeat in showSeats)
            {
                showSeat.Status = ShowSeatStatuses.Booked;
            }

            // Add booking and its booking-seat records
            _context.Bookings.Add(booking);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // Save pending database changes
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
