
using Microsoft.EntityFrameworkCore;
using TrainingManagementSystem.Data;
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Services;

public class CourseService
{
    private readonly AppDbContext db;

    public CourseService(AppDbContext context)
    {
        db = context;
    }

    public async Task<List<Course>> GetAllCourses()
    {
        return await db.Courses.ToListAsync();
    }

    public async Task<Course?> GetCourseByCode(string code)
    {
        return await db.Courses.FindAsync(code);
    }

    public async Task<List<Course>> SearchCourses(string search)
    {
        List<Course> courses = await db.Courses.ToListAsync();
        List<Course> result = new();

        foreach (Course course in courses)
        {
            if (course.CourseCode.Contains(
                    search, StringComparison.OrdinalIgnoreCase) ||
                course.Title.Contains(
                    search, StringComparison.OrdinalIgnoreCase))
            {
                result.Add(course);
            }
        }

        return result;
    }

    public async Task<List<Course>> SortByFee(bool ascending)
    {
        List<Course> courses = await db.Courses.ToListAsync();

        Course[] sorted = courses.ToArray();

        Array.Sort(sorted, (a, b) =>
            ascending
                ? a.Fee.CompareTo(b.Fee)
                : b.Fee.CompareTo(a.Fee));

        return new List<Course>(sorted);
    }

    public async Task<string?> AddCourse(Course course)
    {
        if (string.IsNullOrWhiteSpace(course.CourseCode) ||
            string.IsNullOrWhiteSpace(course.Title))
        {
            return "Course code and title are required.";
        }

        if (course.Fee < 0 ||
            course.DurationDays <= 0 ||
            course.TotalSeats <= 0)
        {
            return "Enter valid fee, duration and seats.";
        }

        if (await db.Courses.FindAsync(course.CourseCode) != null)
        {
            return "Course code already exists.";
        }

        course.EnrolledCount = 0;

        db.Courses.Add(course);
        await db.SaveChangesAsync();

        return null;
    }

    public async Task<string?> UpdateCourse(
        string code, Course updated)
    {
        Course? course = await db.Courses.FindAsync(code);

        if (course == null)
            return "Course not found.";

        if (string.IsNullOrWhiteSpace(updated.Title) ||
            updated.Fee < 0 ||
            updated.DurationDays <= 0 ||
            updated.TotalSeats < course.EnrolledCount)
        {
            return "Invalid course details or seat capacity.";
        }

        course.Title = updated.Title;
        course.Fee = updated.Fee;
        course.DurationDays = updated.DurationDays;
        course.TotalSeats = updated.TotalSeats;

        await db.SaveChangesAsync();

        return null;
    }

    public async Task<string?> DeleteCourse(string code)
    {
        Course? course = await db.Courses.FindAsync(code);

        if (course == null)
            return "Course not found.";

        bool hasHistory = await db.Enrollments
            .AnyAsync(e => e.CourseCode == code);

        if (hasHistory)
            return "Cannot delete a course with enrollment history.";

        db.Courses.Remove(course);
        await db.SaveChangesAsync();

        return null;
    }
}
