using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StyleStore.Api.Models;

namespace StyleStore.Api.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Product>().Property(p => p.Price).HasPrecision(10, 2);
        builder.Entity<Order>().Property(o => o.TotalAmount).HasPrecision(10, 2);
        builder.Entity<OrderItem>().Property(i => i.UnitPrice).HasPrecision(10, 2);
        builder.Entity<Order>()
            .HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
