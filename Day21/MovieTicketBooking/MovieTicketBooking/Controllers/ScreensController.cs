using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Screens;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScreensController : ControllerBase
{
    private readonly IScreenService _screenService;

    public ScreensController(IScreenService screenService)
    {
        _screenService = screenService;
    }

    // GET: api/screens
    // GET: api/screens?theatreId=1
    // Public access
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<ScreenResponse>>> GetAll(
        [FromQuery] int? theatreId)
    {
        if (theatreId.HasValue)
        {
            var theatreScreens =
                await _screenService.GetByTheatreIdAsync(theatreId.Value);

            return Ok(theatreScreens);
        }

        var screens = await _screenService.GetAllAsync();
        return Ok(screens);
    }

    // GET: api/screens/1
    // Public access
    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ScreenResponse>> GetById(int id)
    {
        var screen = await _screenService.GetByIdAsync(id);

        if (screen == null)
            return NotFound(new { message = "Screen not found." });

        return Ok(screen);
    }

    // GET: api/screens/theatre/1
    // Public access
    [AllowAnonymous]
    [HttpGet("theatre/{theatreId:int}")]
    public async Task<ActionResult<List<ScreenResponse>>> GetByTheatre(
        int theatreId)
    {
        var screens =
            await _screenService.GetByTheatreIdAsync(theatreId);

        return Ok(screens);
    }

    // POST: api/screens
    // Admin only
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ScreenResponse>> Create(
        CreateScreenRequest request)
    {
        try
        {
            var screen = await _screenService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = screen.ScreenId },
                screen);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // PUT: api/screens/1
    // Admin only
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ScreenResponse>> Update(
        int id,
        UpdateScreenRequest request)
    {
        var screen = await _screenService.UpdateAsync(id, request);

        if (screen == null)
            return NotFound(new { message = "Screen not found." });

        return Ok(screen);
    }

    // DELETE: api/screens/1
    // Admin only
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _screenService.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = "Screen not found." });

            return Ok(new { message = "Screen deleted successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}