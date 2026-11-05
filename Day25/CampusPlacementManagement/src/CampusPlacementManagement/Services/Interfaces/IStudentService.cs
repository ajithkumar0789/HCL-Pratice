
using CampusPlacementManagement.DTOs.Students;

namespace CampusPlacementManagement.Services.Interfaces;

public interface IStudentService
{
    Task<IEnumerable<StudentResponse>> GetAllAsync();

    Task<StudentResponse?> GetByIdAsync(int id);

    Task<StudentResponse> CreateAsync(CreateStudentRequest request);

    Task<StudentResponse?> UpdateAsync(int id, UpdateStudentRequest request);

    Task<bool> DeleteAsync(int id);
}
