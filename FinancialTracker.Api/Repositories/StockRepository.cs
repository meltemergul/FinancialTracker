using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialTracker.Api.Data;
using FinancialTracker.Api.Models;
using FinancialTracker.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace FinancialTracker.Api.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly AppDbContext _db;
        public StockRepository(AppDbContext db) => _db = db;
        // Repository Pattern: encapsulates EF Core data access behind an abstraction
        // so services stay focused on business logic and become easier to test.
        public Task<Stock?> GetBySymbolAsync(string symbol, CancellationToken ct = default) =>
            _db.Stocks.Include(s => s.PriceSnapshots)
                .FirstOrDefaultAsync(s => s.Symbol == symbol, ct);
        public Task<List<Stock>> GetAllWithSnapshotsAsync(CancellationToken ct = default) =>
            _db.Stocks.Include(s => s.PriceSnapshots).ToListAsync(ct);
        public async Task AddAsync(Stock stock, CancellationToken ct = default) =>
            await _db.Stocks.AddAsync(stock, ct);
        public Task DeleteAsync(Stock stock, CancellationToken ct = default)
        {
            _db.Stocks.Remove(stock);
            return Task.CompletedTask;
        }
        public Task SaveChangesAsync(CancellationToken ct = default) =>
            _db.SaveChangesAsync(ct);
    }
}