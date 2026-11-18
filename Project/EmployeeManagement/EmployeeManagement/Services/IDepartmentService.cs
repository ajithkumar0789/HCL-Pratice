using EmployeeManagement.API.DTOs.Department;

namespace EmployeeManagement.API.Services
{
    public interface IDepartmentService
    {
        Task<DepartmentResponseDto> AddDepartmentAsync(
            CreateDepartmentDto dto);

        Task<List<DepartmentResponseDto>> GetAllDepartmentsAsync();

        Task<DepartmentResponseDto?> GetDepartmentByIdAsync(int id);

        Task<DepartmentResponseDto?> UpdateDepartmentAsync(
            int id,
            UpdateDepartmentDto dto);

        Task<bool> DeleteDepartmentAsync(int id);
    }
}