
using EmployeeManagement.API.Models;

namespace EmployeeManagement.API.Repositories
{
    public interface IEmployeeRepository
    {
        Task AddEmployee(Employee employee);

        Task<List<Employee>> GetAllEmployees();

        Task<Employee?> GetEmployeeById(int id);

        Task<bool> UpdateSalary(int id, decimal salary);

        Task<bool> UpdateEmployee(int id, Employee employee);

        Task<bool> DeleteEmployee(int id);
    }
}
