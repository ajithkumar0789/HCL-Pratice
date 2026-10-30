
using BankingConsoleApp.Model;
using Microsoft.EntityFrameworkCore;

namespace BankingConsoleApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // =====================================
    // TABLE DECLARATIONS
    // =====================================

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();

    public DbSet<Transaction> Transactions => Set<Transaction>();

    public DbSet<Branch> Branches => Set<Branch>();

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<AuditLog> AuditLogs { get; set; }

    public DbSet<Loan> Loans => Set<Loan>();

    // =====================================
    // DATABASE CONNECTION
    // =====================================

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\MSSQLLocalDB;Database=BankingConsoleDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

    // =====================================
    // TABLE RULES AND RELATIONSHIPS
    // =====================================

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // =====================================
        // CUSTOMER TABLE
        // =====================================

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(c => c.CustomerId);

            entity.Property(c => c.FullName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(c => c.Phone)
                .IsRequired()
                .HasMaxLength(15);

            entity.HasIndex(c => c.Email)
                .IsUnique();
        });

        // =====================================
        // BANK ACCOUNT TABLE
        // =====================================

        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.HasKey(a => a.BankAccountId);

            entity.Property(a => a.AccountHolder)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(a => a.AccountNumber)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(a => a.Balance)
                .HasPrecision(18, 2);

            entity.ToTable(t =>
                t.HasCheckConstraint(
                    "CK_BankAccount_Balance",
                    "[Balance] >= 0"));

            entity.HasIndex(a => a.AccountNumber)
                .IsUnique();
        });

        // =====================================
        // TRANSACTION TABLE
        // =====================================

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.TransactionId);

            entity.Property(t => t.TransactionType)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(t => t.Amount)
                .HasPrecision(18, 2);

            entity.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_Transaction_Amount",
                    "[Amount] > 0");

                t.HasCheckConstraint(
                    "CK_Transaction_Type",
                    "[TransactionType] IN ('Deposit', 'Withdrawal')");
            });
        });

        // =====================================
        // BRANCH TABLE
        // =====================================

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(b => b.BranchId);

            entity.Property(b => b.BranchName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(b => b.BranchCode)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(b => b.Address)
                .IsRequired()
                .HasMaxLength(250);

            entity.HasIndex(b => b.BranchCode)
                .IsUnique();
        });

        // =====================================
        // EMPLOYEE TABLE
        // =====================================

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId);

            entity.Property(e => e.EmployeeName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.Phone)
                .IsRequired()
                .HasMaxLength(15);

            entity.Property(e => e.Designation)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(e => e.Email)
                .IsUnique();
        });

        // =====================================
        // LOAN TABLE
        // =====================================

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(l => l.LoanId);

            entity.Property(l => l.LoanType)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(l => l.LoanAmount)
                .HasPrecision(18, 2);

            entity.Property(l => l.InterestRate)
                .HasPrecision(5, 2);

            entity.Property(l => l.Status)
                .IsRequired()
                .HasMaxLength(20);

            entity.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_Loan_Amount",
                    "[LoanAmount] > 0");

                t.HasCheckConstraint(
                    "CK_Loan_InterestRate",
                    "[InterestRate] >= 0 AND [InterestRate] <= 100");

                t.HasCheckConstraint(
                    "CK_Loan_Status",
                    "[Status] IN ('Pending', 'Approved', 'Rejected')");
            });
        });

        // =====================================
        // RELATIONSHIPS
        // =====================================

        // Customer 1 ---- Many BankAccounts

        modelBuilder.Entity<Customer>()
            .HasMany(c => c.BankAccounts)
            .WithOne(a => a.Customer)
            .HasForeignKey(a => a.CustomerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        // Customer 1 ---- Many Loans

        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Loans)
            .WithOne(l => l.Customer)
            .HasForeignKey(l => l.CustomerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        // BankAccount 1 ---- Many Transactions

        modelBuilder.Entity<BankAccount>()
            .HasMany(a => a.Transactions)
            .WithOne(t => t.BankAccount)
            .HasForeignKey(t => t.BankAccountId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Branch 1 ---- Many BankAccounts

        modelBuilder.Entity<Branch>()
            .HasMany(b => b.BankAccounts)
            .WithOne(a => a.Branch)
            .HasForeignKey(a => a.BranchId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        // Branch 1 ---- Many Employees

        modelBuilder.Entity<Branch>()
            .HasMany(b => b.Employees)
            .WithOne(e => e.Branch)
            .HasForeignKey(e => e.BranchId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
