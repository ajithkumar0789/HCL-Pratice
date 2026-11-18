
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.API.Data;
using EmployeeManagement.API.Models;

namespace EmployeeManagement.API.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddEmployee(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            return await _context.Employees
                .Include(e => e.Department)
                .ToListAsync();
        }

        public async Task<Employee?> GetEmployeeById(int id)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        // Update employee salary
        public async Task<bool> UpdateSalary(int id, decimal salary)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return false;

            employee.Salary = salary;

            await _context.SaveChangesAsync();

            return true;
        }

        // Update complete employee details
        public async Task<bool> UpdateEmployee(int id, Employee updatedEmployee)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return false;

            employee.Name = updatedEmployee.Name;
            employee.Email = updatedEmployee.Email;
            employee.Phone = updatedEmployee.Phone;
            employee.DepartmentId = updatedEmployee.DepartmentId;
            employee.Designation = updatedEmployee.Designation;
            employee.Salary = updatedEmployee.Salary;

            await _context.SaveChangesAsync();

            return true;
        }

        // Delete employee
        public async Task<bool> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return false;

            _context.Employees.Remove(employee);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
