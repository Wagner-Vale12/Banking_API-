using BankingApiVerity.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApiVerity.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<BankTransaction> Transactions => Set<BankTransaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>()
            .HasIndex(x => x.Document)
            .IsUnique();

        modelBuilder.Entity<BankTransaction>()
            .HasIndex(x => x.IdempotencyKey)
            .IsUnique();
    }
}
