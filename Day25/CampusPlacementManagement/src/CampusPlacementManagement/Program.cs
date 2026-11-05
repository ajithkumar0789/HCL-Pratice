
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Repositories;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Register controllers
builder.Services.AddControllers();

// 2. Register OpenAPI
builder.Services.AddOpenApi();

// 3. Register SQL Server database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// 4. Register Repository Layer
builder.Services.AddScoped<ICollegeRepository, CollegeRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

// 5. Register Service Layer
builder.Services.AddScoped<ICollegeService, CollegeService>();
builder.Services.AddScoped<IStudentService, StudentService>();

var app = builder.Build();

// 6. Configure OpenAPI for Development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// 7. Configure HTTPS redirection
app.UseHttpsRedirection();

// 8. Configure authorization
app.UseAuthorization();

// 9. Map controller endpoints
app.MapControllers();

// 10. Start the application
app.Run();
