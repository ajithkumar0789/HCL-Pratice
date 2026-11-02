using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface IShowRepository
{
    Task<List<Show>> GetAllAsync(
        DateTime? date, int? movieId, int? theatreId);

    Task<Show?> GetByIdAsync(int id);

    Task<List<Show>> GetByMovieIdAsync(int movieId);

    Task<List<Show>> GetByTheatreIdAsync(int theatreId);

    Task<bool> MovieExistsAsync(int movieId);

    Task<bool> ScreenExistsAsync(int screenId);

    Task<bool> HasOverlapAsync(
        int screenId, DateTime startTime, DateTime endTime, int? excludeShowId = null);

    Task CreateWithSeatsAsync(Show show, List<ShowSeat> showSeats);

    Task UpdateAsync(Show show);

    Task<bool> DeleteAsync(int id);

    Task<bool> HasBookingsAsync(int showId);

    Task<List<ShowSeat>> GetSeatsAsync(int showId, string? status);
}