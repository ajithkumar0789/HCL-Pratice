using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.API.DTOs.Department;
using EmployeeManagement.API.Services;

namespace EmployeeManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _service;

        public DepartmentController(IDepartmentService service)
        {
            _service = service;
        }

        // POST: api/Department
        [HttpPost]
        public async Task<IActionResult> AddDepartment(
            CreateDepartmentDto dto)
        {
            try
            {
                var department = await _service.AddDepartmentAsync(dto);

                return CreatedAtAction(
                    nameof(GetDepartmentById),
                    new { id = department.Id },
                    department
                );
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // GET: api/Department
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments =
                await _service.GetAllDepartmentsAsync();

            return Ok(departments);
        }

        // GET: api/Department/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var department =
                await _service.GetDepartmentByIdAsync(id);

            if (department == null)
                return NotFound(new { message = "Department not found." });

            return Ok(department);
        }

        // PUT: api/Department/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDepartment(
            int id,
            UpdateDepartmentDto dto)
        {
            try
            {
                var department =
                    await _service.UpdateDepartmentAsync(id, dto);

                if (department == null)
                    return NotFound(new { message = "Department not found." });

                return Ok(department);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // DELETE: api/Department/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            bool deleted = await _service.DeleteDepartmentAsync(id);

            if (!deleted)
                return NotFound(new { message = "Department not found." });

            return Ok(new { message = "Department deleted successfully." });
        }
    }
}