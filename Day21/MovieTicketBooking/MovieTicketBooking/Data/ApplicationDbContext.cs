
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Database tables
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Theatre> Theatres { get; set; }
    public DbSet<Screen> Screens { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Show> Shows { get; set; }
    public DbSet<ShowSeat> ShowSeats { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingSeat> BookingSeats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Movie configuration
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("Movies", table =>
            {
                table.HasCheckConstraint(
                    "CK_Movies_DurationMinutes",
                    "[DurationMinutes] > 0");
            });

            entity.HasKey(m => m.MovieId);

            entity.Property(m => m.Title)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(m => m.Description)
                .HasMaxLength(500);

            entity.Property(m => m.DurationMinutes)
                .IsRequired();

            entity.Property(m => m.ReleaseDate)
                .HasColumnType("date");

            entity.Property(m => m.Language)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(m => m.Genre)
                .HasMaxLength(50)
                .IsRequired();
        });

        // Theatre configuration
        modelBuilder.Entity<Theatre>(entity =>
        {
            entity.ToTable("Theatres");
            entity.HasKey(t => t.TheatreId);

            entity.Property(t => t.Name)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(t => t.Location)
                .HasMaxLength(200)
                .IsRequired();
        });

        // Screen configuration
        modelBuilder.Entity<Screen>(entity =>
        {
            entity.ToTable("Screens");
            entity.HasKey(s => s.ScreenId);

            entity.Property(s => s.ScreenName)
                .HasMaxLength(50)
                .IsRequired();

            entity.HasIndex(s => new { s.TheatreId, s.ScreenName })
                .IsUnique();

            entity.HasOne(s => s.Theatre)
                .WithMany(t => t.Screens)
                .HasForeignKey(s => s.TheatreId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Seat configuration
        modelBuilder.Entity<Seat>(entity =>
        {
            entity.ToTable("Seats", table =>
            {
                table.HasCheckConstraint(
                    "CK_Seats_SeatNumber",
                    "[SeatNumber] > 0");
            });

            entity.HasKey(s => s.SeatId);

            entity.Property(s => s.RowName)
                .HasMaxLength(5)
                .IsRequired();

            entity.Property(s => s.SeatType)
                .HasMaxLength(30)
                .IsRequired();

            entity.HasIndex(s => new
            {
                s.ScreenId,
                s.RowName,
                s.SeatNumber
            }).IsUnique();

            entity.HasOne(s => s.Screen)
                .WithMany(s => s.Seats)
                .HasForeignKey(s => s.ScreenId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Show configuration
        modelBuilder.Entity<Show>(entity =>
        {
            entity.ToTable("Shows", table =>
            {
                table.HasCheckConstraint(
                    "CK_Shows_EndTime",
                    "[EndTime] > [StartTime]");

                table.HasCheckConstraint(
                    "CK_Shows_TicketPrice",
                    "[TicketPrice] >= 0");
            });

            entity.HasKey(s => s.ShowId);

            entity.Property(s => s.TicketPrice)
                .HasPrecision(10, 2);

            entity.HasOne(s => s.Movie)
                .WithMany(m => m.Shows)
                .HasForeignKey(s => s.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.Screen)
                .WithMany(s => s.Shows)
                .HasForeignKey(s => s.ScreenId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ShowSeat configuration
        modelBuilder.Entity<ShowSeat>(entity =>
        {
            entity.ToTable("ShowSeats");
            entity.HasKey(s => s.ShowSeatId);

            entity.Property(s => s.Status)
                .HasMaxLength(20)
                .IsRequired();

            entity.HasIndex(s => new { s.ShowId, s.SeatId })
                .IsUnique();

            entity.HasOne(s => s.Show)
                .WithMany(s => s.ShowSeats)
                .HasForeignKey(s => s.ShowId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.Seat)
                .WithMany(s => s.ShowSeats)
                .HasForeignKey(s => s.SeatId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Booking configuration
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("Bookings", table =>
            {
                table.HasCheckConstraint(
                    "CK_Bookings_TotalAmount",
                    "[TotalAmount] >= 0");
            });

            entity.HasKey(b => b.BookingId);

            entity.Property(b => b.BookingReference)
                .HasMaxLength(30)
                .IsRequired();

            entity.HasIndex(b => b.BookingReference)
                .IsUnique();

            entity.Property(b => b.CustomerName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(b => b.CustomerEmail)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(b => b.CustomerPhone)
                .HasMaxLength(15)
                .IsRequired();

            entity.Property(b => b.TotalAmount)
                .HasPrecision(10, 2);

            entity.Property(b => b.Status)
                .HasMaxLength(20)
                .IsRequired();

            entity.HasOne(b => b.Show)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.ShowId)
                .OnDelete(DeleteBehavior.Restrict);

            // Booking belongs to an Identity user
            entity.HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // BookingSeat configuration
        modelBuilder.Entity<BookingSeat>(entity =>
        {
            entity.ToTable("BookingSeats", table =>
            {
                table.HasCheckConstraint(
                    "CK_BookingSeats_Price",
                    "[Price] >= 0");
            });

            entity.HasKey(b => b.BookingSeatId);

            entity.Property(b => b.Price)
                .HasPrecision(10, 2);

            entity.HasOne(b => b.Booking)
                .WithMany(b => b.BookingSeats)
                .HasForeignKey(b => b.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(b => b.Seat)
                .WithMany(s => s.BookingSeats)
                .HasForeignKey(b => b.SeatId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
