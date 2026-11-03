using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface IScreenRepository
{
    Task<List<Screen>> GetAllAsync();

    Task<Screen?> GetByIdAsync(int id);

    Task<List<Screen>> GetByTheatreIdAsync(int theatreId);

    Task AddAsync(Screen screen);

    Task UpdateAsync(Screen screen);

    Task<bool> DeleteAsync(int id);

    Task<bool> HasSeatsAsync(int id);

    Task<bool> HasShowsAsync(int id);

    Task<bool> TheatreExistsAsync(int theatreId);
}