using EmployeeManagement.API.Models;

namespace EmployeeManagement.API.Repositories
{
    public interface IDepartmentRepository
    {
        Task AddAsync(Department department);

        Task<List<Department>> GetAllAsync();

        Task<Department?> GetByIdAsync(int id);

        Task UpdateAsync(Department department);

        Task DeleteAsync(Department department);

        Task<bool> ExistsByNameAsync(string departmentName, int? excludeId = null);
    }
}