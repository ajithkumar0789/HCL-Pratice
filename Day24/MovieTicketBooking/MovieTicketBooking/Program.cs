
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using Scalar.AspNetCore;

using MovieTicketBooking.Repositories;
using MovieTicketBooking.Repositories.Interfaces;

using MovieTicketBooking.Services;
using MovieTicketBooking.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Controllers
builder.Services.AddControllers();

// 2. Register SQL Server Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Register OpenAPI
builder.Services.AddOpenApi();

// 4. Movie Dependency Injection
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieService, MovieService>();

// 5. Theatre Dependency Injection
builder.Services.AddScoped<ITheatreRepository, TheatreRepository>();
builder.Services.AddScoped<ITheatreService, TheatreService>();

// 6. Screen Dependency Injection
builder.Services.AddScoped<IScreenRepository, ScreenRepository>();
builder.Services.AddScoped<IScreenService, ScreenService>();

// 7. Seat Dependency Injection
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<ISeatService, SeatService>();

// 8. Show Dependency Injection
builder.Services.AddScoped<IShowRepository, ShowRepository>();
builder.Services.AddScoped<IShowService, ShowService>();

var app = builder.Build();

// 9. Configure HTTP Request Pipeline
if (app.Environment.IsDevelopment())
{
    // Generate OpenAPI document
    app.MapOpenApi();

    // Enable Scalar API documentation
    app.MapScalarApiReference();
}

// 10. HTTPS Redirection
app.UseHttpsRedirection();

// 11. Authorization
app.UseAuthorization();

// 12. Map Controllers
app.MapControllers();

// 13. Run Application
app.Run();
