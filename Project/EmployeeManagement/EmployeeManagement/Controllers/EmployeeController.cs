
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

        // POST: api/Employee
        [HttpPost]
        public async Task<IActionResult> AddEmployee(
            CreateEmployeeDto dto)
        {
            var employee = await _service.AddEmployee(dto);

            return CreatedAtAction(
                nameof(GetEmployeeById),
                new { id = employee.Id },
                employee
            );
        }

        // GET: api/Employee
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _service.GetAllEmployees();

            return Ok(employees);
        }

        // GET: api/Employee/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _service.GetEmployeeById(id);

            if (employee == null)
                return NotFound("Employee not found.");

            return Ok(employee);
        }
    }
}
