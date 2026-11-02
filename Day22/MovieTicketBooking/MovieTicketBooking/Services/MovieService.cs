using MovieTicketBooking.DTOs.Movies;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _repository;

    public MovieService(IMovieRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<MovieResponse>> GetAllAsync()
    {
        var movies = await _repository.GetAllAsync();

        return movies.Select(MapToResponse).ToList();
    }

    public async Task<MovieResponse?> GetByIdAsync(int id)
    {
        var movie = await _repository.GetByIdAsync(id);

        return movie == null ? null : MapToResponse(movie);
    }

    public async Task<List<MovieResponse>> SearchByTitleAsync(string title)
    {
        var movies = await _repository.SearchByTitleAsync(title.Trim());

        return movies.Select(MapToResponse).ToList();
    }

    public async Task<MovieResponse> CreateAsync(CreateMovieRequest request)
    {
        var movie = new Movie
        {
            Title = request.Title.Trim(),
            Description = request.Description?.Trim(),
            DurationMinutes = request.DurationMinutes,
            ReleaseDate = request.ReleaseDate,
            Language = request.Language.Trim(),
            Genre = request.Genre.Trim()
        };

        await _repository.AddAsync(movie);

        return MapToResponse(movie);
    }

    public async Task<MovieResponse?> UpdateAsync(
        int id,
        UpdateMovieRequest request)
    {
        var movie = await _repository.GetByIdAsync(id);

        if (movie == null)
            return null;

        movie.Title = request.Title.Trim();
        movie.Description = request.Description?.Trim();
        movie.DurationMinutes = request.DurationMinutes;
        movie.ReleaseDate = request.ReleaseDate;
        movie.Language = request.Language.Trim();
        movie.Genre = request.Genre.Trim();

        await _repository.UpdateAsync(movie);

        return MapToResponse(movie);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var movie = await _repository.GetByIdAsync(id);

        if (movie == null)
            return false;

        if (await _repository.HasShowsAsync(id))
        {
            throw new InvalidOperationException(
                "Movie cannot be deleted because shows already exist.");
        }

        return await _repository.DeleteAsync(id);
    }

    private static MovieResponse MapToResponse(Movie movie)
    {
        return new MovieResponse
        {
            MovieId = movie.MovieId,
            Title = movie.Title,
            Description = movie.Description,
            DurationMinutes = movie.DurationMinutes,
            ReleaseDate = movie.ReleaseDate,
            Language = movie.Language,
            Genre = movie.Genre
        };
    }
}