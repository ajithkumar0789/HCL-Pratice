using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StyleStore.Api.DTOs;
using StyleStore.Api.Models;

namespace StyleStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName
        };
        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors.Select(e => e.Description));
        return Ok(await CreateToken(user));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user is null || !await userManager.CheckPasswordAsync(user, dto.Password))
            return Unauthorized(new { message = "Invalid email or password." });
        return Ok(await CreateToken(user));
    }

    private Task<AuthResponseDto> CreateToken(ApplicationUser user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email ?? "")
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            configuration["Jwt:Issuer"], configuration["Jwt:Audience"], claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(configuration["Jwt:ExpiresMinutes"]!)),
            signingCredentials: credentials);
        return Task.FromResult(new AuthResponseDto(new JwtSecurityTokenHandler().WriteToken(token),
            user.FullName, user.Email ?? ""));
    }
}
