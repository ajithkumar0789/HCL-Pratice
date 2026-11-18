
using EmployeeManagement.API.DTOs;

namespace EmployeeManagement.API.Services
{
    public interface IEmployeeService
    {
        Task<EmployeeDto> AddEmployee(CreateEmployeeDto dto);

        Task<List<EmployeeDto>> GetAllEmployees();

        Task<EmployeeDto?> GetEmployeeById(int id);
    }
}
