using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Theatres;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TheatresController : ControllerBase
{
    private readonly ITheatreService _theatreService;

    public TheatresController(ITheatreService theatreService)
    {
        _theatreService = theatreService;
    }

    // GET: api/theatres
    [HttpGet]
    public async Task<ActionResult<List<TheatreResponse>>> GetAll()
    {
        var theatres = await _theatreService.GetAllAsync();
        return Ok(theatres);
    }

    // GET: api/theatres/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TheatreResponse>> GetById(int id)
    {
        var theatre = await _theatreService.GetByIdAsync(id);

        if (theatre == null)
            return NotFound(new { message = "Theatre not found." });

        return Ok(theatre);
    }

    // POST: api/theatres
    [HttpPost]
    public async Task<ActionResult<TheatreResponse>> Create(
        CreateTheatreRequest request)
    {
        var theatre = await _theatreService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = theatre.TheatreId },
            theatre);
    }

    // PUT: api/theatres/1
    [HttpPut("{id:int}")]
    public async Task<ActionResult<TheatreResponse>> Update(
        int id,
        UpdateTheatreRequest request)
    {
        var theatre = await _theatreService.UpdateAsync(id, request);

        if (theatre == null)
            return NotFound(new { message = "Theatre not found." });

        return Ok(theatre);
    }

    // DELETE: api/theatres/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _theatreService.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = "Theatre not found." });

            return Ok(new
            {
                message = "Theatre deleted successfully."
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}