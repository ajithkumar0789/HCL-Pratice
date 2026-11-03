using MovieTicketBooking.DTOs.Screens;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class ScreenService : IScreenService
{
    private readonly IScreenRepository _repository;

    public ScreenService(IScreenRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ScreenResponse>> GetAllAsync()
    {
        var screens = await _repository.GetAllAsync();

        return screens.Select(MapToResponse).ToList();
    }

    public async Task<ScreenResponse?> GetByIdAsync(int id)
    {
        var screen = await _repository.GetByIdAsync(id);

        return screen == null ? null : MapToResponse(screen);
    }

    public async Task<List<ScreenResponse>> GetByTheatreIdAsync(
        int theatreId)
    {
        var screens = await _repository.GetByTheatreIdAsync(theatreId);

        return screens.Select(MapToResponse).ToList();
    }

    public async Task<ScreenResponse> CreateAsync(
        CreateScreenRequest request)
    {
        if (!await _repository.TheatreExistsAsync(request.TheatreId))
        {
            throw new KeyNotFoundException("Theatre not found.");
        }

        var screen = new Screen
        {
            TheatreId = request.TheatreId,
            ScreenName = request.ScreenName.Trim()
        };

        await _repository.AddAsync(screen);

        var savedScreen = await _repository.GetByIdAsync(screen.ScreenId);

        return MapToResponse(savedScreen!);
    }

    public async Task<ScreenResponse?> UpdateAsync(
        int id,
        UpdateScreenRequest request)
    {
        var screen = await _repository.GetByIdAsync(id);

        if (screen == null)
            return null;

        screen.ScreenName = request.ScreenName.Trim();

        await _repository.UpdateAsync(screen);

        return MapToResponse(screen);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var screen = await _repository.GetByIdAsync(id);

        if (screen == null)
            return false;

        if (await _repository.HasSeatsAsync(id) ||
            await _repository.HasShowsAsync(id))
        {
            throw new InvalidOperationException(
                "Screen cannot be deleted because seats or shows already exist.");
        }

        return await _repository.DeleteAsync(id);
    }

    private static ScreenResponse MapToResponse(Screen screen)
    {
        return new ScreenResponse
        {
            ScreenId = screen.ScreenId,
            TheatreId = screen.TheatreId,
            TheatreName = screen.Theatre?.Name ?? string.Empty,
            ScreenName = screen.ScreenName
        };
    }
}