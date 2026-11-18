
using EmployeeManagement.API.DTOs;

namespace EmployeeManagement.API.Services
{
    public interface IEmployeeService
    {
        Task<EmployeeDto> AddEmployee(CreateEmployeeDto dto);

        Task<List<EmployeeDto>> GetAllEmployees();

        Task<EmployeeDto?> GetEmployeeById(int id);

        Task<EmployeeDto?> UpdateSalary(int id, UpdateSalaryDto dto);

        Task<EmployeeDto?> UpdateEmployee(int id, UpdateEmployeeDto dto);

        Task<bool> DeleteEmployee(int id);
    }
}
