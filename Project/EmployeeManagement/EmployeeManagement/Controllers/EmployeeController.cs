
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.API.DTOs;
using EmployeeManagement.API.Services;

namespace EmployeeManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        // Add Employee
        [HttpPost]
        public async Task<IActionResult> AddEmployee(CreateEmployeeDto dto)
        {
            var employee = await _service.AddEmployee(dto);

            return CreatedAtAction(
                nameof(GetEmployeeById),
                new { id = employee.Id },
                employee
            );
        }

        // Get All Employees
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _service.GetAllEmployees();

            return Ok(employees);
        }

        // Get Employee By ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _service.GetEmployeeById(id);

            if (employee == null)
                return NotFound("Employee not found.");

            return Ok(employee);
        }

        // Update Salary
        [HttpPut("{id:int}/salary")]
        public async Task<IActionResult> UpdateSalary(
            int id, UpdateSalaryDto dto)
        {
            var employee = await _service.UpdateSalary(id, dto);

            if (employee == null)
                return NotFound("Employee not found.");

            return Ok(employee);
        }

        // Update Complete Employee
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEmployee(
            int id, UpdateEmployeeDto dto)
        {
            var employee = await _service.UpdateEmployee(id, dto);

            if (employee == null)
                return NotFound("Employee not found.");

            return Ok(employee);
        }

        // Delete Employee
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var deleted = await _service.DeleteEmployee(id);

            if (!deleted)
                return NotFound("Employee not found.");

            return Ok("Employee deleted successfully.");
        }
    }
}
