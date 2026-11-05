
using CampusPlacementManagement.DTOs.Colleges;

namespace CampusPlacementManagement.Services.Interfaces;

public interface ICollegeService
{
    Task<IEnumerable<CollegeResponse>> GetAllAsync();

    Task<CollegeResponse?> GetByIdAsync(int id);

    Task<CollegeResponse> CreateAsync(CreateCollegeRequest request);

    Task<CollegeResponse?> UpdateAsync(int id, UpdateCollegeRequest request);

    Task<bool> DeleteAsync(int id);
}
