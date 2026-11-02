
using Microsoft.AspNetCore.Identity;
using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public BookingService(
        IBookingRepository bookingRepository,
        UserManager<ApplicationUser> userManager)
    {
        _bookingRepository = bookingRepository;
        _userManager = userManager;
    }

    public async Task<BookingResponse> CreateBookingAsync(
        CreateBookingRequest request,
        string userId)
    {
        // Find the authenticated user
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            throw new InvalidOperationException("User not found.");

        if (string.IsNullOrWhiteSpace(user.Email))
            throw new InvalidOperationException(
                "Your account must have an email address to create a booking.");

        var show = await _bookingRepository
            .GetShowWithSeatsAsync(request.ShowId);

        if (show == null)
            throw new InvalidOperationException("Show not found.");

        if (request.SeatIds == null || request.SeatIds.Count == 0)
            throw new InvalidOperationException("Select at least one seat.");

        if (request.SeatIds.Distinct().Count() != request.SeatIds.Count)
            throw new InvalidOperationException("Duplicate seats selected.");

        var selectedSeats = show.ShowSeats
            .Where(ss => request.SeatIds.Contains(ss.SeatId))
            .ToList();

        if (selectedSeats.Count != request.SeatIds.Count)
            throw new InvalidOperationException("Invalid seat selection.");

        if (selectedSeats.Any(ss =>
            ss.Status != ShowSeatStatuses.Available))
        {
            throw new InvalidOperationException(
                "One or more seats are unavailable.");
        }

        var booking = new Booking
        {
            BookingReference = "BK" +
                Guid.NewGuid().ToString("N")[..12].ToUpperInvariant(),

            ShowId = request.ShowId,

            // Always use the authenticated user's ID
            UserId = user.Id,

            // Never trust customer identity from the request
            CustomerName = string.IsNullOrWhiteSpace(user.FullName)
                ? user.UserName ?? "Customer"
                : user.FullName,

            CustomerEmail = user.Email,

            CustomerPhone = request.CustomerPhone ?? string.Empty,

            BookingDate = DateTime.UtcNow,

            TotalAmount = show.TicketPrice * request.SeatIds.Count,

            Status = BookingStatuses.Confirmed,

            BookingSeats = request.SeatIds.Select(seatId =>
                new BookingSeat
                {
                    SeatId = seatId,
                    Price = show.TicketPrice
                }).ToList()
        };

        await _bookingRepository.AddBookingAsync(booking);

        var savedBooking = await _bookingRepository
            .GetBookingByIdAsync(booking.BookingId);

        if (savedBooking == null)
            throw new InvalidOperationException(
                "Booking could not be retrieved.");

        return MapToResponse(savedBooking);
    }

    public async Task<BookingResponse?> GetBookingByIdAsync(
        int bookingId,
        string userId,
        bool isAdmin)
    {
        var booking = await _bookingRepository
            .GetBookingByIdAsync(bookingId);

        if (booking == null)
            return null;

        // Customers can access only their own bookings
        if (!isAdmin && booking.UserId != userId)
            return null;

        return MapToResponse(booking);
    }

    public async Task<BookingResponse?> GetBookingByReferenceAsync(
        string bookingReference,
        string userId,
        bool isAdmin)
    {
        var booking = await _bookingRepository
            .GetBookingByReferenceAsync(bookingReference);

        if (booking == null)
            return null;

        // Customers can access only their own bookings
        if (!isAdmin && booking.UserId != userId)
            return null;

        return MapToResponse(booking);
    }

    private static BookingResponse MapToResponse(Booking booking)
    {
        return new BookingResponse
        {
            BookingId = booking.BookingId,
            BookingReference = booking.BookingReference,
            ShowId = booking.ShowId,
            CustomerName = booking.CustomerName,
            CustomerEmail = booking.CustomerEmail,
            CustomerPhone = booking.CustomerPhone,
            BookingDate = booking.BookingDate,
            TotalAmount = booking.TotalAmount,
            Status = booking.Status,

            Seats = booking.BookingSeats.Select(bs =>
                new BookingSeatResponse
                {
                    SeatId = bs.SeatId,
                    RowName = bs.Seat?.RowName ?? string.Empty,
                    SeatNumber = bs.Seat?.SeatNumber ?? 0,
                    SeatType = bs.Seat?.SeatType ?? string.Empty,
                    Price = bs.Price
                }).ToList()
        };
    }
}
