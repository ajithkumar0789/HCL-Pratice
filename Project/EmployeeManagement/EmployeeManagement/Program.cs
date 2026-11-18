
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.API.Data;
using EmployeeManagement.API.Services;
using EmployeeManagement.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Database connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Employee Dependency Injection
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// Department Dependency Injection
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Check actual database connection
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var connection = db.Database.GetDbConnection();

    Console.WriteLine("----------------------------------");
    Console.WriteLine("DATABASE CONNECTION INFORMATION");
    Console.WriteLine("SERVER: " + connection.DataSource);
    Console.WriteLine("DATABASE: " + connection.Database);
    Console.WriteLine("----------------------------------");
}

app.UseCors("AngularPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
