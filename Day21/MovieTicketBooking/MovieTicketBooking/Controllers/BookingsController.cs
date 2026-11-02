using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    // POST: api/bookings
    [HttpPost]
    public async Task<ActionResult<BookingResponse>> CreateBooking(
        CreateBookingRequest request)
    {
        try
        {
            var booking = await _bookingService.CreateBookingAsync(request);

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
    [HttpGet("{id}")]
    public async Task<ActionResult<BookingResponse>> GetBookingById(int id)
    {
        var booking = await _bookingService.GetBookingByIdAsync(id);

        if (booking == null)
            return NotFound(new { message = "Booking not found." });

        return Ok(booking);
    }

    // GET: api/bookings/reference/BK123456
    [HttpGet("reference/{bookingReference}")]
    public async Task<ActionResult<BookingResponse>> GetBookingByReference(
        string bookingReference)
    {
        var booking = await _bookingService
            .GetBookingByReferenceAsync(bookingReference);

        if (booking == null)
            return NotFound(new { message = "Booking not found." });

        return Ok(booking);
    }
}