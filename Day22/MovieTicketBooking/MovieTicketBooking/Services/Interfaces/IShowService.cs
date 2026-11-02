using MovieTicketBooking.DTOs.Shows;

namespace MovieTicketBooking.Services.Interfaces;

public interface IShowService
{
    Task<List<ShowResponse>> GetAllAsync(
        DateTime? date, int? movieId, int? theatreId);

    Task<ShowResponse?> GetByIdAsync(int id);

    Task<List<ShowResponse>> GetByMovieIdAsync(int movieId);

    Task<List<ShowResponse>> GetByTheatreIdAsync(int theatreId);

    Task<ShowResponse> CreateAsync(CreateShowRequest request);

    Task<ShowResponse?> UpdateAsync(int id, UpdateShowRequest request);

    Task<bool> DeleteAsync(int id);

    Task<List<ShowSeatResponse>> GetSeatsAsync(int showId, string? status);
}