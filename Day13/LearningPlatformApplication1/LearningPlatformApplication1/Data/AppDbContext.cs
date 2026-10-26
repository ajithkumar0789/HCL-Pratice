
using Microsoft.EntityFrameworkCore;
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Learner> Learners { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>()
                .HasKey(c => c.CourseCode);

            modelBuilder.Entity<Learner>()
                .HasKey(l => l.LearnerId);

            modelBuilder.Entity<Enrollment>()
                .HasKey(e => e.EnrollmentId);

            modelBuilder.Entity<Course>()
                .Property(c => c.Fee)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Enrollment>()
                .Property(e => e.BaseFee)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Enrollment>()
                .Property(e => e.DiscountAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Enrollment>()
                .Property(e => e.FinalFee)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Enrollment>()
                .Property(e => e.Status)
                .HasConversion<string>();
        }
    }
}
