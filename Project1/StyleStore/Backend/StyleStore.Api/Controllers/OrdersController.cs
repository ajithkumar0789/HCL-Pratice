using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StyleStore.Api.Data;
using StyleStore.Api.DTOs;
using StyleStore.Api.Models;

namespace StyleStore.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class OrdersController(ApplicationDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMine()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var orders = await db.Orders.AsNoTracking()
            .Include(o => o.Items)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
        return Ok(orders);
    }

    [HttpPost]
    public async Task<IActionResult> Place(CreateOrderDto dto)
    {
        if (dto.Items is null || dto.Items.Count == 0)
            return BadRequest(new { message = "Your cart is empty." });
        if (dto.Items.Any(i => i.Quantity < 1 || string.IsNullOrWhiteSpace(i.Size)))
            return BadRequest(new { message = "Each item needs a valid size and quantity." });

        var productIds = dto.Items.Select(i => i.ProductId).Distinct().ToList();
        var products = await db.Products.Where(p => productIds.Contains(p.Id)).ToDictionaryAsync(p => p.Id);
        if (dto.Items.Any(i => !products.ContainsKey(i.ProductId)))
            return BadRequest(new { message = "One or more products no longer exist." });

        var order = new Order { UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)! };
        foreach (var item in dto.Items)
        {
            var product = products[item.ProductId];
            order.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Size = item.Size,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });
            order.TotalAmount += product.Price * item.Quantity;
        }
        db.Orders.Add(order);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMine), new { id = order.Id }, order);
    }
}
