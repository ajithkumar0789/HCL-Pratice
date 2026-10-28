
namespace ECommerceManagement.Models;

public class Customer
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = "";

    public string Email { get; set; } = "";

    public string PhoneNumber { get; set; } = "";

    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; } = true;
}
