
using CampusPlacementManagement.DTOs.Students;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    // GET: api/students
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentResponse>>> GetAll()
    {
        var students = await _studentService.GetAllAsync();
        return Ok(students);
    }

    // GET: api/students/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudentResponse>> GetById(int id)
    {
        var student = await _studentService.GetByIdAsync(id);

        if (student == null)
            return NotFound(new { message = "Student not found." });

        return Ok(student);
    }

    // POST: api/students
    [HttpPost]
    public async Task<ActionResult<StudentResponse>> Create(
        [FromBody] CreateStudentRequest request)
    {
        try
        {
            var student = await _studentService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = student.StudentId },
                student);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/students/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult<StudentResponse>> Update(
        int id,
        [FromBody] UpdateStudentRequest request)
    {
        try
        {
            var student = await _studentService.UpdateAsync(id, request);

            if (student == null)
                return NotFound(new { message = "Student not found." });

            return Ok(student);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/students/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _studentService.DeleteAsync(id);

        if (!deleted)
            return NotFound(new { message = "Student not found." });

        return NoContent();
    }
}
