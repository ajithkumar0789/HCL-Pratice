
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
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee?> GetEmployeeById(int id)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
