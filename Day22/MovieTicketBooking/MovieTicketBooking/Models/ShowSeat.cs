namespace MovieTicketBooking.Models;

public class ShowSeat
{
    public int ShowSeatId { get; set; }

    public int ShowId { get; set; }

    public int SeatId { get; set; }

    public string Status { get; set; } = ShowSeatStatuses.Available;

    public Show? Show { get; set; }

    public Seat? Seat { get; set; }
}

public static class ShowSeatStatuses
{
    public const string Available = "Available";

    public const string Booked = "Booked";
}