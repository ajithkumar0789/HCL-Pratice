
using EmployeeManagement.API.DTOs;
using EmployeeManagement.API.Models;
using EmployeeManagement.API.Repositories;

namespace EmployeeManagement.API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        // Add Employee
        public async Task<Employee> AddEmployee(CreateEmployeeDto dto)
        {
            var employee = new Employee
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Department = dto.Department,
                Designation = dto.Designation,
                Salary = dto.Salary
            };

            await _repository.AddEmployee(employee);

            return employee;
        }

        // Display All Employees
        public async Task<List<Employee>> GetAllEmployees()
        {
            return await _repository.GetAllEmployees();
        }

        // Search Employee By ID
        public async Task<Employee?> GetEmployeeById(int id)
        {
            return await _repository.GetEmployeeById(id);
        }
    }
}
