
using CampusPlacementManagement.DTOs.Colleges;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CollegesController : ControllerBase
{
    private readonly ICollegeService _collegeService;

    public CollegesController(ICollegeService collegeService)
    {
        _collegeService = collegeService;
    }

    // GET: api/colleges
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CollegeResponse>>> GetAll()
    {
        var colleges = await _collegeService.GetAllAsync();
        return Ok(colleges);
    }

    // GET: api/colleges/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CollegeResponse>> GetById(int id)
    {
        var college = await _collegeService.GetByIdAsync(id);

        if (college == null)
            return NotFound(new { message = "College not found." });

        return Ok(college);
    }

    // POST: api/colleges
    [HttpPost]
    public async Task<ActionResult<CollegeResponse>> Create(
        [FromBody] CreateCollegeRequest request)
    {
        try
        {
            var college = await _collegeService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = college.CollegeId },
                college);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/colleges/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult<CollegeResponse>> Update(
        int id,
        [FromBody] UpdateCollegeRequest request)
    {
        try
        {
            var college = await _collegeService.UpdateAsync(id, request);

            if (college == null)
                return NotFound(new { message = "College not found." });

            return Ok(college);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/colleges/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _collegeService.DeleteAsync(id);

        if (!deleted)
            return NotFound(new { message = "College not found." });

        return NoContent();
    }
}
