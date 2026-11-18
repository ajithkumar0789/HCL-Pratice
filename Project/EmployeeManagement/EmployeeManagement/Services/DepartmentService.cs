using EmployeeManagement.API.DTOs.Department;
using EmployeeManagement.API.Models;
using EmployeeManagement.API.Repositories;

namespace EmployeeManagement.API.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repository;

        public DepartmentService(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        // Add Department
        public async Task<DepartmentResponseDto> AddDepartmentAsync(
            CreateDepartmentDto dto)
        {
            string name = dto.DepartmentName.Trim();

            if (await _repository.ExistsByNameAsync(name))
            {
                throw new InvalidOperationException(
                    "Department name already exists.");
            }

            var department = new Department
            {
                DepartmentName = name,
                Description = dto.Description?.Trim()
            };

            await _repository.AddAsync(department);

            return MapToResponse(department);
        }

        // Get All Departments
        public async Task<List<DepartmentResponseDto>>
            GetAllDepartmentsAsync()
        {
            var departments = await _repository.GetAllAsync();

            return departments.Select(MapToResponse).ToList();
        }

        // Get Department By ID
        public async Task<DepartmentResponseDto?>
            GetDepartmentByIdAsync(int id)
        {
            var department = await _repository.GetByIdAsync(id);

            if (department == null)
                return null;

            return MapToResponse(department);
        }

        // Update Department
        public async Task<DepartmentResponseDto?> UpdateDepartmentAsync(
            int id,
            UpdateDepartmentDto dto)
        {
            var department = await _repository.GetByIdAsync(id);

            if (department == null)
                return null;

            string name = dto.DepartmentName.Trim();

            if (await _repository.ExistsByNameAsync(name, id))
            {
                throw new InvalidOperationException(
                    "Department name already exists.");
            }

            department.DepartmentName = name;
            department.Description = dto.Description?.Trim();

            await _repository.UpdateAsync(department);

            return MapToResponse(department);
        }

        // Delete Department
        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var department = await _repository.GetByIdAsync(id);

            if (department == null)
                return false;

            await _repository.DeleteAsync(department);

            return true;
        }

        // Convert Entity to Response DTO
        private static DepartmentResponseDto MapToResponse(
            Department department)
        {
            return new DepartmentResponseDto
            {
                Id = department.Id,
                DepartmentName = department.DepartmentName,
                Description = department.Description
            };
        }
    }
}
