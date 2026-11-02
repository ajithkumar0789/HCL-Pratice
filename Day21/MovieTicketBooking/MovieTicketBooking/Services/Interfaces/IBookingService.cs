using MovieTicketBooking.DTOs.Bookings;

namespace MovieTicketBooking.Services.Interfaces;

public interface IBookingService
{
    Task<BookingResponse> CreateBookingAsync(
        CreateBookingRequest request);

    Task<BookingResponse?> GetBookingByIdAsync(int bookingId);

    Task<BookingResponse?> GetBookingByReferenceAsync(
        string bookingReference);
}