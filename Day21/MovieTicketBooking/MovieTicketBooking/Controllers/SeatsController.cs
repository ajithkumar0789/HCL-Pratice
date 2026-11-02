using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Seats;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeatsController : ControllerBase
{
    private readonly ISeatService _seatService;

    public SeatsController(ISeatService seatService)
    {
        _seatService = seatService;
    }

    // GET: api/seats/screen/1
    [HttpGet("screen/{screenId:int}")]
    public async Task<ActionResult<List<SeatResponse>>> GetByScreen(
        int screenId)
    {
        var seats = await _seatService.GetByScreenIdAsync(screenId);
        return Ok(seats);
    }

    // GET: api/seats/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SeatResponse>> GetById(int id)
    {
        var seat = await _seatService.GetByIdAsync(id);

        if (seat == null)
            return NotFound(new { message = "Seat not found." });

        return Ok(seat);
    }

    // POST: api/seats
    [HttpPost]
    public async Task<ActionResult<SeatResponse>> Create(
        CreateSeatRequest request)
    {
        try
        {
            var seat = await _seatService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = seat.SeatId },
                seat);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // POST: api/seats/bulk
    [HttpPost("bulk")]
    public async Task<ActionResult<List<SeatResponse>>> BulkCreate(
        BulkCreateSeatsRequest request)
    {
        try
        {
            var seats = await _seatService.BulkCreateAsync(request);

            return Ok(seats);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // PUT: api/seats/1
    [HttpPut("{id:int}")]
    public async Task<ActionResult<SeatResponse>> Update(
        int id,
        UpdateSeatRequest request)
    {
        try
        {
            var seat = await _seatService.UpdateAsync(id, request);

            if (seat == null)
                return NotFound(new { message = "Seat not found." });

            return Ok(seat);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // DELETE: api/seats/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _seatService.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = "Seat not found." });

            return Ok(new { message = "Seat deleted successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}