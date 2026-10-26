
using Microsoft.EntityFrameworkCore;
using TrainingManagementSystem.Data;
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Services;

public class EnrollmentService
{
    private readonly AppDbContext db;
    private readonly FeeCalculator feeCalculator;

    public EnrollmentService(
        AppDbContext context,
        FeeCalculator calculator)
    {
        db = context;
        feeCalculator = calculator;
    }

    public async Task<List<Enrollment>> GetAllEnrollments()
    {
        return await db.Enrollments.ToListAsync();
    }

    public async Task<Enrollment?> GetEnrollmentById(int id)
    {
        return await db.Enrollments.FindAsync(id);
    }

    public async Task<(string? Error, Enrollment? Record)> Enroll(
        string learnerId, string courseCode)
    {
        Learner? learner = await db.Learners.FindAsync(learnerId);

        if (learner == null)
            return ("Learner not found. Register first.", null);

        Course? course = await db.Courses.FindAsync(courseCode);

        if (course == null)
            return ("Course not found.", null);

        List<Enrollment> records = await db.Enrollments.ToListAsync();

        foreach (Enrollment item in records)
        {
            if (item.LearnerId.Equals(
                    learnerId, StringComparison.OrdinalIgnoreCase) &&
                item.CourseCode.Equals(
                    courseCode, StringComparison.OrdinalIgnoreCase) &&
                item.Status == EnrollmentStatus.Active)
            {
                return ("Already actively enrolled in this course.", null);
            }
        }

        if (course.AvailableSeats() <= 0)
            return ("No seats available.", null);

        decimal fee = feeCalculator.CalculateFee(course.Fee);

        Enrollment enrollment = new(
     0,
     learner.LearnerId,
     course.CourseCode,
     fee,
     0m,
     fee
 );

        course.EnrolledCount++;
        db.Enrollments.Add(enrollment);

        await db.SaveChangesAsync();

        return (null, enrollment);
    }

    public async Task<string?> CancelEnrollment(int id)
    {
        Enrollment? enrollment = await db.Enrollments.FindAsync(id);

        if (enrollment == null)
            return "Enrollment not found.";

        if (enrollment.Status != EnrollmentStatus.Active)
            return "Enrollment is not active.";

        Course? course = await db.Courses.FindAsync(
            enrollment.CourseCode);

        if (course == null)
            return "Associated course not found.";

        if (course.EnrolledCount <= 0)
            return "Invalid course seat count.";

        enrollment.Status = EnrollmentStatus.Cancelled;
        course.EnrolledCount--;

        await db.SaveChangesAsync();

        return null;
    }
}
