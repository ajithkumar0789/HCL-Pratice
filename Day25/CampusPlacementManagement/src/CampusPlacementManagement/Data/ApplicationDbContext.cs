
using CampusPlacementManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<College> Colleges => Set<College>();

    public DbSet<Student> Students => Set<Student>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<College>(entity =>
        {
            entity.HasKey(c => c.CollegeId);

            entity.Property(c => c.CollegeName)
                .IsRequired()
                .HasMaxLength(150);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(s => s.StudentId);

            entity.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.HasIndex(s => s.Email)
                .IsUnique();

            entity.Property(s => s.CGPA)
                .HasPrecision(4, 2);

            entity.HasOne(s => s.College)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.CollegeId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
