
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    // Get authenticated user's ID from JWT
    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");
    }

    // POST: api/bookings
    // Any authenticated customer or admin can create a booking.
    [HttpPost]
    public async Task<ActionResult<BookingResponse>> CreateBooking(
        CreateBookingRequest request)
    {
        var userId = GetCurrentUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        try
        {
            var booking = await _bookingService.CreateBookingAsync(
                request,
                userId);

            return CreatedAtAction(
                nameof(GetBookingById),
                new { id = booking.BookingId },
                booking);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET: api/bookings/1
    // Customers can view their own bookings.
    // Admins can view all bookings.
    [HttpGet("{id}")]
    public async Task<ActionResult<BookingResponse>> GetBookingById(int id)
    {
        var userId = GetCurrentUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var booking = await _bookingService.GetBookingByIdAsync(
            id,
            userId,
            User.IsInRole("Admin"));

        if (booking == null)
            return NotFound(new { message = "Booking not found." });

        return Ok(booking);
    }

    // GET: api/bookings/reference/BK123456
    [HttpGet("reference/{bookingReference}")]
    public async Task<ActionResult<BookingResponse>> GetBookingByReference(
        string bookingReference)
    {
        var userId = GetCurrentUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var booking = await _bookingService.GetBookingByReferenceAsync(
            bookingReference,
            userId,
            User.IsInRole("Admin"));

        if (booking == null)
            return NotFound(new { message = "Booking not found." });

        return Ok(booking);
    }
}
