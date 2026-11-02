
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Shows;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShowsController : ControllerBase
{
    private readonly IShowService _showService;

    public ShowsController(IShowService showService)
    {
        _showService = showService;
    }

    // GET: api/shows
    // GET: api/shows?date=2026-11-02&movieId=1&theatreId=1
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<ShowResponse>>> GetAll(
        [FromQuery] DateTime? date,
        [FromQuery] int? movieId,
        [FromQuery] int? theatreId)
    {
        var shows = await _showService.GetAllAsync(
            date, movieId, theatreId);

        return Ok(shows);
    }

    // GET: api/shows/1
    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ShowResponse>> GetById(int id)
    {
        var show = await _showService.GetByIdAsync(id);

        if (show == null)
            return NotFound(new { message = "Show not found." });

        return Ok(show);
    }

    // GET: api/shows/movie/1
    [AllowAnonymous]
    [HttpGet("movie/{movieId:int}")]
    public async Task<ActionResult<List<ShowResponse>>> GetByMovie(
        int movieId)
    {
        var shows = await _showService.GetByMovieIdAsync(movieId);
        return Ok(shows);
    }

    // GET: api/shows/theatre/1
    [AllowAnonymous]
    [HttpGet("theatre/{theatreId:int}")]
    public async Task<ActionResult<List<ShowResponse>>> GetByTheatre(
        int theatreId)
    {
        var shows = await _showService.GetByTheatreIdAsync(theatreId);
        return Ok(shows);
    }

    // GET: api/shows/search?date=2026-11-02
    [AllowAnonymous]
    [HttpGet("search")]
    public async Task<ActionResult<List<ShowResponse>>> Search(
        [FromQuery] DateTime date)
    {
        var shows = await _showService.GetAllAsync(date, null, null);
        return Ok(shows);
    }

    // POST: api/shows
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ShowResponse>> Create(
        CreateShowRequest request)
    {
        try
        {
            var show = await _showService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = show.ShowId },
                show);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/shows/1
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ShowResponse>> Update(
        int id,
        UpdateShowRequest request)
    {
        try
        {
            var show = await _showService.UpdateAsync(id, request);

            if (show == null)
                return NotFound(new { message = "Show not found." });

            return Ok(show);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/shows/1
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _showService.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = "Show not found." });

            return Ok(new { message = "Show deleted successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // GET: api/shows/1/seats
    [AllowAnonymous]
    [HttpGet("{showId:int}/seats")]
    public async Task<ActionResult<List<ShowSeatResponse>>> GetSeats(
        int showId)
    {
        try
        {
            var seats = await _showService.GetSeatsAsync(showId, null);
            return Ok(seats);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // GET: api/shows/1/seats/available
    [AllowAnonymous]
    [HttpGet("{showId:int}/seats/available")]
    public async Task<ActionResult<List<ShowSeatResponse>>> GetAvailableSeats(
        int showId)
    {
        try
        {
            var seats = await _showService.GetSeatsAsync(
                showId, "Available");

            return Ok(seats);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // GET: api/shows/1/seats/booked
    [AllowAnonymous]
    [HttpGet("{showId:int}/seats/booked")]
    public async Task<ActionResult<List<ShowSeatResponse>>> GetBookedSeats(
        int showId)
    {
        try
        {
            var seats = await _showService.GetSeatsAsync(
                showId, "Booked");

            return Ok(seats);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
