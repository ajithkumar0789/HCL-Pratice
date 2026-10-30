using BankingConsoleApp.Model;
using BankingConsoleApp.Repositories;

namespace BankingConsoleApp.Services;

public class EmployeeService
{
    private readonly EmployeeRepository _repository;

    public EmployeeService(EmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task AddEmployeeAsync(Employee employee)
    {
        if (string.IsNullOrWhiteSpace(employee.EmployeeName))
            throw new ArgumentException("Employee name is required.");

        if (string.IsNullOrWhiteSpace(employee.Email))
            throw new ArgumentException("Employee email is required.");

        if (employee.BranchId <= 0)
            throw new ArgumentException("A valid branch ID is required.");

        await _repository.AddAsync(employee);
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
    {
        if (employeeId <= 0)
            throw new ArgumentException("Employee ID must be greater than zero.");

        return await _repository.GetByIdAsync(employeeId);
    }

    public async Task<List<Employee>> GetAllEmployeesAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task UpdateEmployeeAsync(Employee employee)
    {
        if (employee.EmployeeId <= 0)
            throw new ArgumentException("A valid employee ID is required.");

        if (string.IsNullOrWhiteSpace(employee.EmployeeName))
            throw new ArgumentException("Employee name is required.");

        await _repository.UpdateAsync(employee);
    }

    public async Task DeleteEmployeeAsync(int employeeId)
    {
        if (employeeId <= 0)
            throw new ArgumentException("Employee ID must be greater than zero.");

        await _repository.DeleteAsync(employeeId);
    }
}