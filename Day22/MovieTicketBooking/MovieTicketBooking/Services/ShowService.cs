using MovieTicketBooking.DTOs.Shows;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class ShowService : IShowService
{
    private readonly IShowRepository _repository;
    private readonly ISeatRepository _seatRepository;

    public ShowService(
        IShowRepository repository,
        ISeatRepository seatRepository)
    {
        _repository = repository;
        _seatRepository = seatRepository;
    }

    public async Task<List<ShowResponse>> GetAllAsync(
        DateTime? date, int? movieId, int? theatreId)
    {
        var shows = await _repository.GetAllAsync(date, movieId, theatreId);

        return shows.Select(MapToResponse).ToList();
    }

    public async Task<ShowResponse?> GetByIdAsync(int id)
    {
        var show = await _repository.GetByIdAsync(id);

        return show == null ? null : MapToResponse(show);
    }

    public async Task<List<ShowResponse>> GetByMovieIdAsync(int movieId)
    {
        var shows = await _repository.GetByMovieIdAsync(movieId);

        return shows.Select(MapToResponse).ToList();
    }

    public async Task<List<ShowResponse>> GetByTheatreIdAsync(int theatreId)
    {
        var shows = await _repository.GetByTheatreIdAsync(theatreId);

        return shows.Select(MapToResponse).ToList();
    }

    public async Task<ShowResponse> CreateAsync(CreateShowRequest request)
    {
        ValidateTime(request.StartTime, request.EndTime);

        if (!await _repository.MovieExistsAsync(request.MovieId))
            throw new KeyNotFoundException("Movie not found.");

        if (!await _repository.ScreenExistsAsync(request.ScreenId))
            throw new KeyNotFoundException("Screen not found.");

        if (await _repository.HasOverlapAsync(
            request.ScreenId, request.StartTime, request.EndTime))
        {
            throw new InvalidOperationException(
                "Another show already exists during this time on this screen.");
        }

        var seats = await _seatRepository.GetByScreenIdAsync(request.ScreenId);

        if (seats.Count == 0)
            throw new InvalidOperationException(
                "Create seats for this screen before creating a show.");

        var show = new Show
        {
            MovieId = request.MovieId,
            ScreenId = request.ScreenId,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            TicketPrice = request.TicketPrice
        };

        var showSeats = seats.Select(seat => new ShowSeat
        {
            SeatId = seat.SeatId,
            Status = ShowSeatStatuses.Available
        }).ToList();

        await _repository.CreateWithSeatsAsync(show, showSeats);

        var savedShow = await _repository.GetByIdAsync(show.ShowId);

        return MapToResponse(savedShow!);
    }

    public async Task<ShowResponse?> UpdateAsync(
        int id, UpdateShowRequest request)
    {
        ValidateTime(request.StartTime, request.EndTime);

        var show = await _repository.GetByIdAsync(id);

        if (show == null)
            return null;

        if (await _repository.HasBookingsAsync(id))
            throw new InvalidOperationException(
                "A show with booking history cannot be updated.");

        if (!await _repository.MovieExistsAsync(request.MovieId))
            throw new KeyNotFoundException("Movie not found.");

        if (!await _repository.ScreenExistsAsync(request.ScreenId))
            throw new KeyNotFoundException("Screen not found.");

        if (await _repository.HasOverlapAsync(
            request.ScreenId, request.StartTime, request.EndTime, id))
        {
            throw new InvalidOperationException(
                "Another show already exists during this time on this screen.");
        }

        if (request.ScreenId != show.ScreenId)
        {
            throw new InvalidOperationException(
                "A show cannot be moved to another screen after creation.");
        }

        show.MovieId = request.MovieId;
        show.StartTime = request.StartTime;
        show.EndTime = request.EndTime;
        show.TicketPrice = request.TicketPrice;

        await _repository.UpdateAsync(show);

        return MapToResponse(show);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var show = await _repository.GetByIdAsync(id);

        if (show == null)
            return false;

        if (await _repository.HasBookingsAsync(id))
            throw new InvalidOperationException(
                "A show with booking history cannot be deleted.");

        return await _repository.DeleteAsync(id);
    }

    public async Task<List<ShowSeatResponse>> GetSeatsAsync(
        int showId, string? status)
    {
        if (await _repository.GetByIdAsync(showId) == null)
            throw new KeyNotFoundException("Show not found.");

        var seats = await _repository.GetSeatsAsync(showId, status);

        return seats.Select(ss => new ShowSeatResponse
        {
            ShowSeatId = ss.ShowSeatId,
            ShowId = ss.ShowId,
            SeatId = ss.SeatId,
            RowName = ss.Seat?.RowName ?? string.Empty,
            SeatNumber = ss.Seat?.SeatNumber ?? 0,
            SeatType = ss.Seat?.SeatType ?? string.Empty,
            Status = ss.Status
        }).ToList();
    }

    private static void ValidateTime(DateTime startTime, DateTime endTime)
    {
        if (endTime <= startTime)
            throw new ArgumentException(
                "Show end time must be later than start time.");
    }

    private static ShowResponse MapToResponse(Show show)
    {
        return new ShowResponse
        {
            ShowId = show.ShowId,
            MovieId = show.MovieId,
            MovieTitle = show.Movie?.Title ?? string.Empty,
            ScreenId = show.ScreenId,
            ScreenName = show.Screen?.ScreenName ?? string.Empty,
            TheatreId = show.Screen?.TheatreId ?? 0,
            TheatreName = show.Screen?.Theatre?.Name ?? string.Empty,
            StartTime = show.StartTime,
            EndTime = show.EndTime,
            TicketPrice = show.TicketPrice
        };
    }
}