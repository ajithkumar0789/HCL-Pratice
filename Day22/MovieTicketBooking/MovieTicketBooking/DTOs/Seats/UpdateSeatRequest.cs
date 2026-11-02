using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Seats;

public class UpdateSeatRequest
{
    [Required]
    [StringLength(10)]
    public string RowName { get; set; } = string.Empty;

    [Range(1, 1000)]
    public int SeatNumber { get; set; }

    [Required]
    [StringLength(30)]
    public string SeatType { get; set; } = string.Empty;
}