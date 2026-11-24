
using System.Security.Claims;
using Enrollora.Api.DTOs;
using Enrollora.Api.Models;
using Enrollora.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enrollora.Api.Controllers;

[ApiController]
[Route("api/students")]
[Authorize]
public sealed class StudentsController(IStudentService service) : ControllerBase
{
    // Get all students (Administrator)
    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public Task<PagedResult<StudentResponse>> GetAll(
        [FromQuery] StudentQuery query,
        CancellationToken ct)
    {
        return service.GetStudentsAsync(query, ct);
    }

    // Get student by ID (Administrator)
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Administrator")]
    public Task<StudentResponse> GetById(
        int id,
        CancellationToken ct)
    {
        return service.GetByIdAsync(id, ct);
    }

    // Add student (Administrator)
    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Add(
        UpdateStudentRequest request,
        CancellationToken ct)
    {
        var student = await service.AddAsync(request, ct);

        return CreatedAtAction(
            nameof(GetById),
            new { id = student.StudentId },
            student);
    }

    // Get logged-in student's profile
    [HttpGet("me")]
    [Authorize(Roles = "Student")]
    public Task<StudentResponse> GetMe(CancellationToken ct)
    {
        return service.GetMeAsync(CurrentStudentId(), ct);
    }

    // Update logged-in student's profile
    [HttpPut("me")]
    [Authorize(Roles = "Student")]
    public Task<StudentResponse> UpdateMe(
        UpdateStudentRequest request,
        CancellationToken ct)
    {
        return service.UpdateMeAsync(CurrentStudentId(), request, ct);
    }

    // Update student by ID (Administrator)
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Administrator")]
    public Task<StudentResponse> Update(
        int id,
        UpdateStudentRequest request,
        CancellationToken ct)
    {
        return service.UpdateAsync(id, request, ct);
    }

    // Approve student (Administrator)
    [HttpPut("{id:int}/approve")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Approve(
        int id,
        CancellationToken ct)
    {
        await service.ReviewAsync(id, StudentStatus.Approved, ct);

        return Ok(new ApiMessage("Student approved."));
    }

    // Reject student (Administrator)
    [HttpPut("{id:int}/reject")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Reject(
        int id,
        CancellationToken ct)
    {
        await service.ReviewAsync(id, StudentStatus.Rejected, ct);

        return Ok(new ApiMessage("Student registration rejected."));
    }

    // Delete student (Administrator)
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken ct)
    {
        await service.DeleteAsync(id, ct);

        return Ok(new ApiMessage("Student deleted."));
    }

    private int CurrentStudentId() =>
        int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException(
                "Student identity is missing."));
}
