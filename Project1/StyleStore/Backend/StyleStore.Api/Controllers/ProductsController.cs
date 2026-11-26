using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StyleStore.Api.Data;

namespace StyleStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(ApplicationDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? category)
    {
        var query = db.Products.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(category) && category != "All")
            query = query.Where(p => p.Category == category);
        return Ok(await query.OrderByDescending(p => p.IsFeatured).ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        return product is null ? NotFound() : Ok(product);
    }
}
