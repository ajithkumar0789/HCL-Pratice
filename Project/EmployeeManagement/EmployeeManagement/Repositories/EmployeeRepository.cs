
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

        // Add Employee
        public async Task AddEmployee(Employee employee)
        {
            await _context.Employees.AddAsync(employee);

            await _context.SaveChangesAsync();
        }

        // Display All Employees with Department
        public async Task<List<Employee>> GetAllEmployees()
        {
            return await _context.Employees
                .Include(e => e.Department)
                .ToListAsync();
        }

        // Search Employee By ID with Department
        public async Task<Employee?> GetEmployeeById(int id)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
