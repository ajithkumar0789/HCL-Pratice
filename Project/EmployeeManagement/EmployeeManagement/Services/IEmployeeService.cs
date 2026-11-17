using EmployeeManagement.API.DTOs;
using EmployeeManagement.API.Models;

namespace EmployeeManagement.API.Services
{
    public interface IEmployeeService
    {
        Task<Employee> AddEmployee(CreateEmployeeDto dto);
        Task<List<Employee>> GetAllEmployees();
        Task<Employee?> GetEmployeeById(int id);
    }
}