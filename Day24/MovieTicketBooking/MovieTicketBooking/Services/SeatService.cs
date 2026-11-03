using MovieTicketBooking.DTOs.Seats;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class SeatService : ISeatService
{
    private readonly ISeatRepository _repository;

    public SeatService(ISeatRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<SeatResponse>> GetByScreenIdAsync(int screenId)
    {
        var seats = await _repository.GetByScreenIdAsync(screenId);

        return seats.Select(MapToResponse).ToList();
    }

    public async Task<SeatResponse?> GetByIdAsync(int id)
    {
        var seat = await _repository.GetByIdAsync(id);

        return seat == null ? null : MapToResponse(seat);
    }

    public async Task<SeatResponse> CreateAsync(CreateSeatRequest request)
    {
        if (!await _repository.ScreenExistsAsync(request.ScreenId))
            throw new KeyNotFoundException("Screen not found.");

        var seat = new Seat
        {
            ScreenId = request.ScreenId,
            RowName = request.RowName.Trim(),
            SeatNumber = request.SeatNumber,
            SeatType = request.SeatType.Trim()
        };

        await _repository.AddAsync(seat);

        return MapToResponse(seat);
    }

    public async Task<List<SeatResponse>> BulkCreateAsync(
        BulkCreateSeatsRequest request)
    {
        if (!await _repository.ScreenExistsAsync(request.ScreenId))
            throw new KeyNotFoundException("Screen not found.");

        var duplicateSeats = request.Seats
            .GroupBy(s => new
            {
                RowName = s.RowName.Trim().ToUpper(),
                s.SeatNumber
            })
            .Any(g => g.Count() > 1);

        if (duplicateSeats)
            throw new InvalidOperationException(
                "Duplicate row and seat numbers were provided.");

        var existingSeats =
            await _repository.GetByScreenIdAsync(request.ScreenId);

        var hasExistingDuplicate = request.Seats.Any(newSeat =>
            existingSeats.Any(existing =>
                existing.RowName.Equals(
                    newSeat.RowName.Trim(),
                    StringComparison.OrdinalIgnoreCase)
                && existing.SeatNumber == newSeat.SeatNumber));

        if (hasExistingDuplicate)
            throw new InvalidOperationException(
                "One or more seats already exist in this screen.");

        var seats = request.Seats.Select(item => new Seat
        {
            ScreenId = request.ScreenId,
            RowName = item.RowName.Trim(),
            SeatNumber = item.SeatNumber,
            SeatType = item.SeatType.Trim()
        }).ToList();

        await _repository.AddRangeAsync(seats);

        return seats.Select(MapToResponse).ToList();
    }

    public async Task<SeatResponse?> UpdateAsync(
        int id,
        UpdateSeatRequest request)
    {
        var seat = await _repository.GetByIdAsync(id);

        if (seat == null)
            return null;

        var existingSeats =
            await _repository.GetByScreenIdAsync(seat.ScreenId);

        var duplicate = existingSeats.Any(s =>
            s.SeatId != id
            && s.RowName.Equals(
                request.RowName.Trim(),
                StringComparison.OrdinalIgnoreCase)
            && s.SeatNumber == request.SeatNumber);

        if (duplicate)
            throw new InvalidOperationException(
                "A seat with this row and number already exists.");

        if (await _repository.HasHistoryAsync(id))
            throw new InvalidOperationException(
                "A seat with show or booking history cannot be updated.");

        seat.RowName = request.RowName.Trim();
        seat.SeatNumber = request.SeatNumber;
        seat.SeatType = request.SeatType.Trim();

        await _repository.UpdateAsync(seat);

        return MapToResponse(seat);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var seat = await _repository.GetByIdAsync(id);

        if (seat == null)
            return false;

        if (await _repository.HasHistoryAsync(id))
            throw new InvalidOperationException(
                "Seat cannot be deleted because show or booking history exists.");

        return await _repository.DeleteAsync(id);
    }

    private static SeatResponse MapToResponse(Seat seat)
    {
        return new SeatResponse
        {
            SeatId = seat.SeatId,
            ScreenId = seat.ScreenId,
            RowName = seat.RowName,
            SeatNumber = seat.SeatNumber,
            SeatType = seat.SeatType
        };
    }
}