using Microsoft.EntityFrameworkCore;
using TrainingManagementSystem.Data;
using TrainingManagementSystem.Models;
using TrainingManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

string connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "DefaultConnection is missing from appsettings.json.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<LearnerService>();
builder.Services.AddScoped<FeeCalculator>();
builder.Services.AddScoped<EnrollmentService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();


// Initialize database and seed courses.

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await db.Database.EnsureCreatedAsync();

    if (!await db.Courses.AnyAsync())
    {
        db.Courses.AddRange(
            new Course("C101", "C# Programming", 6000m, 30, 10),
            new Course("C102", "Python Programming", 5000m, 25, 10),
            new Course("C103", "Web Development", 7000m, 40, 8),
            new Course("C104", "SQL Database", 4000m, 20, 12),
            new Course("C105", "Cloud Computing", 8000m, 45, 5)
        );

        await db.SaveChangesAsync();
    }
}


// COURSE: READ

app.MapGet("/api/courses", async (
    string? search,
    string? sort,
    CourseService service) =>
{
    List<Course> courses = await service.GetAllCourses();

    if (!string.IsNullOrWhiteSpace(search))
    {
        List<Course> matches = new();

        foreach (Course course in courses)
        {
            if (course.Title.Contains(
                search, StringComparison.OrdinalIgnoreCase) ||
                course.CourseCode.Contains(
                search, StringComparison.OrdinalIgnoreCase))
            {
                matches.Add(course);
            }
        }

        courses = matches;
    }

    Course[] sorted = courses.ToArray();

    if (sort == "asc")
    {
        Array.Sort(sorted, (a, b) => a.Fee.CompareTo(b.Fee));
    }
    else if (sort == "desc")
    {
        Array.Sort(sorted, (a, b) => b.Fee.CompareTo(a.Fee));
    }

    return Results.Ok(sorted);
});

app.MapGet("/api/courses/{code}", async (
    string code,
    CourseService service) =>
{
    Course? course = await service.GetCourseByCode(code);

    return course == null
        ? Results.NotFound(new { message = "Course not found." })
        : Results.Ok(course);
});


// COURSE: CREATE

app.MapPost("/api/courses", async (
    CourseRequest request,
    CourseService service) =>
{
    Course course = new(
        request.CourseCode ?? "",
        request.Title ?? "",
        request.Fee,
        request.DurationDays,
        request.TotalSeats);

    string? error = await service.AddCourse(course);

    return error != null
        ? Results.BadRequest(new { message = error })
        : Results.Ok(new { message = "Course created successfully." });
});


// COURSE: UPDATE

app.MapPut("/api/courses/{code}", async (
    string code,
    CourseRequest request,
    CourseService service) =>
{
    Course updated = new(
        code,
        request.Title ?? "",
        request.Fee,
        request.DurationDays,
        request.TotalSeats);

    string? error = await service.UpdateCourse(code, updated);

    return error != null
        ? Results.BadRequest(new { message = error })
        : Results.Ok(new { message = "Course updated successfully." });
});


// COURSE: DELETE

app.MapDelete("/api/courses/{code}", async (
    string code,
    CourseService service) =>
{
    string? error = await service.DeleteCourse(code);

    return error != null
        ? Results.BadRequest(new { message = error })
        : Results.Ok(new { message = "Course deleted successfully." });
});


// LEARNER: READ

app.MapGet("/api/learners", async (LearnerService service) =>
{
    return Results.Ok(await service.GetAllLearners());
});

app.MapGet("/api/learners/{id}", async (
    string id,
    LearnerService service) =>
{
    Learner? learner = await service.GetLearnerById(id);

    return learner == null
        ? Results.NotFound(new { message = "Learner not found." })
        : Results.Ok(learner);
});


// LEARNER: CREATE

app.MapPost("/api/learners", async (
    LearnerRequest request,
    LearnerService service) =>
{
    Learner learner = new(
        request.LearnerId ?? "",
        request.Name ?? "",
        request.ContactNumber ?? "",
        request.Email ?? "");

    string? error = await service.RegisterLearner(learner);

    return error != null
        ? Results.BadRequest(new { message = error })
        : Results.Ok(new { message = "Learner registered successfully." });
});


// LEARNER: UPDATE

app.MapPut("/api/learners/{id}", async (
    string id,
    LearnerRequest request,
    LearnerService service) =>
{
    Learner updated = new(
        id,
        request.Name ?? "",
        request.ContactNumber ?? "",
        request.Email ?? "");

    string? error = await service.UpdateLearner(id, updated);

    return error != null
        ? Results.BadRequest(new { message = error })
        : Results.Ok(new { message = "Learner updated successfully." });
});


// LEARNER: DELETE

app.MapDelete("/api/learners/{id}", async (
    string id,
    LearnerService service) =>
{
    string? error = await service.DeleteLearner(id);

    return error != null
        ? Results.BadRequest(new { message = error })
        : Results.Ok(new { message = "Learner deleted successfully." });
});


// ENROLLMENT: READ

app.MapGet("/api/enrollments", async (
    EnrollmentService service) =>
{
    return Results.Ok(await service.GetAllEnrollments());
});


// ENROLLMENT: CREATE

app.MapPost("/api/enrollments", async (
    EnrollmentRequest request,
    EnrollmentService service) =>
{
    var result = await service.Enroll(
        request.LearnerId ?? "",
        request.CourseCode ?? "");

    if (result.Error != null)
    {
        return Results.BadRequest(new { message = result.Error });
    }

    return Results.Ok(new
    {
        message = "Enrollment successful.",
        enrollment = result.Record
    });
});


// ENROLLMENT: CANCEL

app.MapPost("/api/enrollments/{id:int}/cancel", async (
    int id,
    EnrollmentService service) =>
{
    string? error = await service.CancelEnrollment(id);

    return error != null
        ? Results.BadRequest(new { message = error })
        : Results.Ok(new { message = "Enrollment cancelled successfully." });
});

app.Run();


public class CourseRequest
{
    public string? CourseCode { get; set; }
    public string? Title { get; set; }
    public decimal Fee { get; set; }
    public int DurationDays { get; set; }
    public int TotalSeats { get; set; }
}

public class LearnerRequest
{
    public string? LearnerId { get; set; }
    public string? Name { get; set; }
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
}

public class EnrollmentRequest
{
    public string? LearnerId { get; set; }
    public string? CourseCode { get; set; }
}