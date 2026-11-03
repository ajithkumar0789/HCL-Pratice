using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface IMovieRepository
{
    Task<List<Movie>> GetAllAsync();

    Task<Movie?> GetByIdAsync(int id);

    Task<List<Movie>> SearchByTitleAsync(string title);

    Task AddAsync(Movie movie);

    Task UpdateAsync(Movie movie);

    Task<bool> DeleteAsync(int id);

    Task<bool> HasShowsAsync(int id);
}