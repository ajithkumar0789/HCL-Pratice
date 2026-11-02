
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface IBookingRepository
{
    Task<Show?> GetShowWithSeatsAsync(int showId);

    Task<Booking?> GetBookingByIdAsync(int bookingId);

    Task<Booking?> GetBookingByReferenceAsync(string bookingReference);

    Task AddBookingAsync(Booking booking);

    Task SaveChangesAsync();
}
