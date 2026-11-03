namespace MovieTicketBooking.Models;

public class Booking
{
    public int BookingId { get; set; }

    public string BookingReference { get; set; } = string.Empty;

    public int ShowId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string CustomerEmail { get; set; } = string.Empty;

    public string CustomerPhone { get; set; } = string.Empty;

    public DateTime BookingDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = BookingStatuses.Confirmed;

    public Show? Show { get; set; }

    public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();
}

public static class BookingStatuses
{
    public const string Confirmed = "Confirmed";

    public const string Cancelled = "Cancelled";
}