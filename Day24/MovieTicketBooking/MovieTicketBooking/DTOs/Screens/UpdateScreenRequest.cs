using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Screens;

public class UpdateScreenRequest
{
    [Required]
    [StringLength(100)]
    public string ScreenName { get; set; } = string.Empty;
}