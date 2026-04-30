using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Stock> Stocks => Set<Stock>();
        public DbSet<PriceSnapshot> PriceSnapshots => Set<PriceSnapshot>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stock>()
                .HasIndex(s => s.Symbol)
                .IsUnique();
            modelBuilder.Entity<Stock>()
                .HasMany(s => s.PriceSnapshots)
                .WithOne(p => p.Stock)
                .HasForeignKey(p => p.StockId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Stock>()
                .Property(s => s.Symbol)
                .HasMaxLength(10);
            modelBuilder.Entity<Stock>()
                .Property(s => s.CompanyName)
                .HasMaxLength(200);
        }
    }

}