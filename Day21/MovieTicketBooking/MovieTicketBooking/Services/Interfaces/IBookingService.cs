
using MovieTicketBooking.DTOs.Bookings;

namespace MovieTicketBooking.Services.Interfaces;

public interface IBookingService
{
    Task<BookingResponse> CreateBookingAsync(
        CreateBookingRequest request,
        string userId);

    Task<BookingResponse?> GetBookingByIdAsync(
        int bookingId,
        string userId,
        bool isAdmin);

    Task<BookingResponse?> GetBookingByReferenceAsync(
        string bookingReference,
        string userId,
        bool isAdmin);
}
