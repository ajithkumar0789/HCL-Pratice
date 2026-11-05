
using CampusPlacementManagement.DTOs.Students;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly ICollegeRepository _collegeRepository;

    public StudentService(
        IStudentRepository studentRepository,
        ICollegeRepository collegeRepository)
    {
        _studentRepository = studentRepository;
        _collegeRepository = collegeRepository;
    }

    public async Task<IEnumerable<StudentResponse>> GetAllAsync()
    {
        var students = await _studentRepository.GetAllAsync();

        return students.Select(MapToResponse);
    }

    public async Task<StudentResponse?> GetByIdAsync(int id)
    {
        var student = await _studentRepository.GetByIdAsync(id);

        return student == null ? null : MapToResponse(student);
    }

    public async Task<StudentResponse> CreateAsync(CreateStudentRequest request)
    {
        var college = await _collegeRepository.GetByIdAsync(request.CollegeId);

        if (college == null)
            throw new ArgumentException("College not found.");

        var students = await _studentRepository.GetAllAsync();

        if (students.Any(s =>
            s.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException("Email is already registered.");

        var student = new Student
        {
            FullName = request.FullName,
            Email = request.Email,
            Phone = request.Phone,
            Department = request.Department,
            CGPA = request.CGPA,
            CollegeId = request.CollegeId
        };

        await _studentRepository.AddAsync(student);
        await _studentRepository.SaveChangesAsync();

        student.College = college;

        return MapToResponse(student);
    }

    public async Task<StudentResponse?> UpdateAsync(
        int id, UpdateStudentRequest request)
    {
        var student = await _studentRepository.GetByIdAsync(id);

        if (student == null)
            return null;

        var college = await _collegeRepository.GetByIdAsync(request.CollegeId);

        if (college == null)
            throw new ArgumentException("College not found.");

        var students = await _studentRepository.GetAllAsync();

        if (students.Any(s =>
            s.StudentId != id &&
            s.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException("Email is already registered.");

        student.FullName = request.FullName;
        student.Email = request.Email;
        student.Phone = request.Phone;
        student.Department = request.Department;
        student.CGPA = request.CGPA;
        student.CollegeId = request.CollegeId;
        student.College = college;

        _studentRepository.Update(student);
        await _studentRepository.SaveChangesAsync();

        return MapToResponse(student);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var student = await _studentRepository.GetByIdAsync(id);

        if (student == null)
            return false;

        _studentRepository.Delete(student);
        await _studentRepository.SaveChangesAsync();

        return true;
    }

    private static StudentResponse MapToResponse(Student student)
    {
        return new StudentResponse
        {
            StudentId = student.StudentId,
            FullName = student.FullName,
            Email = student.Email,
            Phone = student.Phone,
            Department = student.Department,
            CGPA = student.CGPA,
            CollegeId = student.CollegeId,
            CollegeName = student.College?.CollegeName ?? string.Empty
        };
    }
}
