using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface ISeatRepository
{
    Task<List<Seat>> GetByScreenIdAsync(int screenId);

    Task<Seat?> GetByIdAsync(int id);

    Task AddAsync(Seat seat);

    Task AddRangeAsync(List<Seat> seats);

    Task UpdateAsync(Seat seat);

    Task<bool> DeleteAsync(int id);

    Task<bool> ScreenExistsAsync(int screenId);

    Task<bool> HasHistoryAsync(int seatId);
}
