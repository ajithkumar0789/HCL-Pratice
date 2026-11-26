using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StyleStore.Api.Data;
using StyleStore.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key is missing.");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => options.AddPolicy("Angular", policy =>
    policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("Angular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Create database tables and seed sample products for the first run.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.EnsureCreatedAsync();
    if (!await db.Products.AnyAsync())
    {
        db.Products.AddRange(
            new Product { Name = "Classic White T-Shirt", Category = "Men", Description = "Soft everyday cotton T-shirt.", Price = 499, ImageUrl = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=700", AvailableSizes = "S,M,L,XL", IsFeatured = true },
            new Product { Name = "Relaxed Denim Jacket", Category = "Women", Description = "A versatile denim layer for everyday outfits.", Price = 1499, ImageUrl = "https://images.unsplash.com/photo-1543076447-215ad9ba6923?w=700", AvailableSizes = "S,M,L", IsFeatured = true },
            new Product { Name = "Everyday Hoodie", Category = "Men", Description = "Comfortable fleece hoodie.", Price = 1199, ImageUrl = "https://images.unsplash.com/photo-1556821840-3a63f95609a7?w=700", AvailableSizes = "M,L,XL", IsFeatured = true },
            new Product { Name = "Floral Summer Dress", Category = "Women", Description = "Lightweight dress for warm days.", Price = 1799, ImageUrl = "https://images.unsplash.com/photo-1595777457583-95e059d581b8?w=700", AvailableSizes = "S,M,L", IsFeatured = false }
        );
        await db.SaveChangesAsync();
    }
}
app.Run();
