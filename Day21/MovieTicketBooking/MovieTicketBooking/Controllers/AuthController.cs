
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using MovieTicketBooking.DTOs.Auth;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    // POST: api/auth/register
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);

        if (existingUser != null)
            return BadRequest(new { message = "Email already registered." });

        var existingUsername =
            await _userManager.FindByNameAsync(request.UserName);

        if (existingUsername != null)
            return BadRequest(new { message = "Username already exists." });

        // Ensure Customer role exists
        if (!await _roleManager.RoleExistsAsync("Customer"))
        {
            var roleResult =
                await _roleManager.CreateAsync(new IdentityRole("Customer"));

            if (!roleResult.Succeeded)
                return StatusCode(500, new { message = "Could not create Customer role." });
        }

        var user = new ApplicationUser
        {
            FullName = request.FullName.Trim(),
            UserName = request.UserName.Trim(),
            Email = request.Email.Trim()
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return BadRequest(new
            {
                errors = result.Errors.Select(e => e.Description)
            });

        var customerResult =
            await _userManager.AddToRoleAsync(user, "Customer");

        if (!customerResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);

            return StatusCode(500, new
            {
                message = "Could not assign Customer role."
            });
        }

        return Ok(new
        {
            message = "Registration successful.",
            userId = user.Id,
            userName = user.UserName,
            role = "Customer"
        });
    }

    // POST: api/auth/login
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = request.UserNameOrEmail.Contains('@')
            ? await _userManager.FindByEmailAsync(request.UserNameOrEmail)
            : await _userManager.FindByNameAsync(request.UserNameOrEmail);

        if (user == null ||
            !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return Unauthorized(new
            {
                message = "Invalid username/email or password."
            });
        }

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty)
        };

        claims.AddRange(roles.Select(role =>
            new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!));

        var credentials = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddMinutes(
            double.Parse(_configuration["Jwt:ExpiryMinutes"]!));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        return Ok(new AuthResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration,
            UserId = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Roles = roles
        });
    }
}
