
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
        public async Task<EmployeeDto> AddEmployee(CreateEmployeeDto dto)
        {
            var employee = new Employee
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                DepartmentId = dto.DepartmentId,
                Designation = dto.Designation,
                Salary = dto.Salary
            };

            await _repository.AddEmployee(employee);

            var savedEmployee =
                await _repository.GetEmployeeById(employee.Id);

            return MapToDto(savedEmployee!);
        }

        // Display All Employees
        public async Task<List<EmployeeDto>> GetAllEmployees()
        {
            var employees = await _repository.GetAllEmployees();

            return employees.Select(MapToDto).ToList();
        }

        // Search Employee By ID
        public async Task<EmployeeDto?> GetEmployeeById(int id)
        {
            var employee = await _repository.GetEmployeeById(id);

            if (employee == null)
                return null;

            return MapToDto(employee);
        }

        // Update Employee Salary
        public async Task<EmployeeDto?> UpdateSalary(
            int id, UpdateSalaryDto dto)
        {
            var updated =
                await _repository.UpdateSalary(id, dto.Salary);

            if (!updated)
                return null;

            var employee = await _repository.GetEmployeeById(id);

            return employee == null ? null : MapToDto(employee);
        }

        // Update Complete Employee Details
        public async Task<EmployeeDto?> UpdateEmployee(
            int id, UpdateEmployeeDto dto)
        {
            var employee = new Employee
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                DepartmentId = dto.DepartmentId,
                Designation = dto.Designation,
                Salary = dto.Salary
            };

            var updated =
                await _repository.UpdateEmployee(id, employee);

            if (!updated)
                return null;

            var updatedEmployee =
                await _repository.GetEmployeeById(id);

            return updatedEmployee == null
                ? null
                : MapToDto(updatedEmployee);
        }

        // Delete Employee
        public async Task<bool> DeleteEmployee(int id)
        {
            return await _repository.DeleteEmployee(id);
        }

        // Convert Employee Model to Employee DTO
        private static EmployeeDto MapToDto(Employee employee)
        {
            return new EmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Phone = employee.Phone,
                DepartmentId = employee.DepartmentId,
                DepartmentName =
                    employee.Department?.DepartmentName ?? string.Empty,
                Designation = employee.Designation,
                Salary = employee.Salary
            };
        }
    }
}
