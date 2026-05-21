using Microsoft.EntityFrameworkCore;
using AkkuAppsAi.Models;

namespace AkkuAppsAi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<CoinTransaction> CoinTransactions => Set<CoinTransaction>();
    public DbSet<DigitalGood> DigitalGoods => Set<DigitalGood>();
    public DbSet<Purchase> Purchases => Set<Purchase>();
    public DbSet<CoinPackage> CoinPackages => Set<CoinPackage>();
    public DbSet<CoinPurchaseOrder> CoinPurchaseOrders => Set<CoinPurchaseOrder>();
    public DbSet<PageVisit> PageVisits => Set<PageVisit>();
    public DbSet<UserPromotion> UserPromotions => Set<UserPromotion>();
    public DbSet<NewsBlog> NewsBlogs => Set<NewsBlog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.EmailHash).IsUnique();
            entity.Property(e => e.CoinBalance).HasPrecision(18, 4);
            entity.Property(e => e.WalletBalance).HasPrecision(18, 4);
        });

        modelBuilder.Entity<CoinTransaction>(entity =>
        {
            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.Amount).HasPrecision(18, 4);
            entity.Property(e => e.BalanceAfter).HasPrecision(18, 4);
        });

        modelBuilder.Entity<DigitalGood>(entity =>
        {
            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(e => e.Category);
            entity.Property(e => e.PriceInCoins).HasPrecision(18, 4);
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<DigitalGood>()
                .WithMany()
                .HasForeignKey(e => e.GoodId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.Property(e => e.CoinsPaid).HasPrecision(18, 4);
            entity.Property(e => e.AdminCommission).HasPrecision(18, 4);
        });

        modelBuilder.Entity<CoinPackage>(entity =>
        {
            entity.Property(e => e.Coins).HasPrecision(18, 4);
            entity.Property(e => e.PriceINR).HasPrecision(18, 4);
        });

        modelBuilder.Entity<CoinPurchaseOrder>(entity =>
        {
            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne<CoinPackage>()
                .WithMany()
                .HasForeignKey(e => e.PackageId)
                .OnDelete(DeleteBehavior.SetNull);
            entity.Property(e => e.Coins).HasPrecision(18, 4);
            entity.Property(e => e.AmountINR).HasPrecision(18, 4);
        });

        modelBuilder.Entity<UserPromotion>(entity =>
        {
            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.CoinsCharged).HasPrecision(18, 4);
        });

        modelBuilder.Entity<PageVisit>(entity =>
        {
            entity.HasIndex(e => e.VisitedAt);
            entity.HasIndex(e => new { e.IPAddress, e.SessionId, e.VisitedAt });
        });

        modelBuilder.Entity<NewsBlog>(entity =>
        {
            entity.HasIndex(e => e.Slug).IsUnique();
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.PublishedAt);
        });
    }
}
