using ECommerceManagement.Models;
using ECommerceManagement.Repositories;

namespace ECommerceManagement.Services;

public class OrderService
{
    private readonly OrderRepository repository = new();

    public List<Order> GetAll() => repository.GetAll();

    public int CreateOrder(int customerId, List<OrderItem> items)
    {
        if (customerId <= 0)
            throw new ArgumentException("Enter a valid customer ID.");

        if (items == null || items.Count == 0)
            throw new ArgumentException("Add at least one product.");

        return repository.CreateOrder(customerId, items);
    }

    public bool UpdateStatus(int orderId, string status)
    {
        if (orderId <= 0)
            throw new ArgumentException("Enter a valid order ID.");

        return repository.UpdateStatus(orderId, status);
    }
}