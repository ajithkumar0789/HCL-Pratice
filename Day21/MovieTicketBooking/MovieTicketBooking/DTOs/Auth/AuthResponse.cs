namespace MovieTicketBooking.DTOs.Auth;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;

    public DateTime Expiration { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public IList<string> Roles { get; set; } = new List<string>();
}