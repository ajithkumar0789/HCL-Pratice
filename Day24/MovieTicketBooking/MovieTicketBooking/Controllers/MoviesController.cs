using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Movies;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    // GET: api/movies
    [HttpGet]
    public async Task<ActionResult<List<MovieResponse>>> GetAll()
    {
        var movies = await _movieService.GetAllAsync();
        return Ok(movies);
    }

    // GET: api/movies/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<MovieResponse>> GetById(int id)
    {
        var movie = await _movieService.GetByIdAsync(id);

        if (movie == null)
            return NotFound(new { message = "Movie not found." });

        return Ok(movie);
    }

    // GET: api/movies/search?title=leo
    [HttpGet("search")]
    public async Task<ActionResult<List<MovieResponse>>> Search(
        [FromQuery] string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return BadRequest(new { message = "Search title is required." });

        var movies = await _movieService.SearchByTitleAsync(title);

        return Ok(movies);
    }

    // POST: api/movies
    [HttpPost]
    public async Task<ActionResult<MovieResponse>> Create(
        CreateMovieRequest request)
    {
        var movie = await _movieService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = movie.MovieId },
            movie);
    }

    // PUT: api/movies/1
    [HttpPut("{id:int}")]
    public async Task<ActionResult<MovieResponse>> Update(
        int id,
        UpdateMovieRequest request)
    {
        var movie = await _movieService.UpdateAsync(id, request);

        if (movie == null)
            return NotFound(new { message = "Movie not found." });

        return Ok(movie);
    }

    // DELETE: api/movies/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _movieService.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = "Movie not found." });

            return Ok(new { message = "Movie deleted successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}