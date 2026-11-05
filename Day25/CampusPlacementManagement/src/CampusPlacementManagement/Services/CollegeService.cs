
using CampusPlacementManagement.DTOs.Colleges;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services;

public class CollegeService : ICollegeService
{
    private readonly ICollegeRepository _repository;

    public CollegeService(ICollegeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CollegeResponse>> GetAllAsync()
    {
        var colleges = await _repository.GetAllAsync();

        return colleges.Select(c => new CollegeResponse
        {
            CollegeId = c.CollegeId,
            CollegeName = c.CollegeName,
            Address = c.Address,
            ContactEmail = c.ContactEmail,
            IsActive = c.IsActive
        });
    }

    public async Task<CollegeResponse?> GetByIdAsync(int id)
    {
        var college = await _repository.GetByIdAsync(id);

        if (college == null)
            return null;

        return MapToResponse(college);
    }

    public async Task<CollegeResponse> CreateAsync(CreateCollegeRequest request)
    {
        var college = new College
        {
            CollegeName = request.CollegeName,
            Address = request.Address,
            ContactEmail = request.ContactEmail,
            IsActive = true
        };

        await _repository.AddAsync(college);
        await _repository.SaveChangesAsync();

        return MapToResponse(college);
    }

    public async Task<CollegeResponse?> UpdateAsync(int id, UpdateCollegeRequest request)
    {
        var college = await _repository.GetByIdAsync(id);

        if (college == null)
            return null;

        college.CollegeName = request.CollegeName;
        college.Address = request.Address;
        college.ContactEmail = request.ContactEmail;
        college.IsActive = request.IsActive;

        _repository.Update(college);
        await _repository.SaveChangesAsync();

        return MapToResponse(college);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var college = await _repository.GetByIdAsync(id);

        if (college == null)
            return false;

        _repository.Delete(college);
        await _repository.SaveChangesAsync();

        return true;
    }

    private static CollegeResponse MapToResponse(College college)
    {
        return new CollegeResponse
        {
            CollegeId = college.CollegeId,
            CollegeName = college.CollegeName,
            Address = college.Address,
            ContactEmail = college.ContactEmail,
            IsActive = college.IsActive
        };
    }
}
