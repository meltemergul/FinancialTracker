using FinancialTracker.Api.Data;
using FinancialTracker.Api.Models;
using FinancialTracker.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.Api.Repositories
{
    public class PriceSnapshotRepository : IPriceSnapshotRepository
    {
        private readonly AppDbContext _db;
        public PriceSnapshotRepository(AppDbContext db) => _db = db;
        public async Task AddAsync(PriceSnapshot snapshot, CancellationToken ct = default) =>
            await _db.PriceSnapshots.AddAsync(snapshot, ct);
        public async Task<List<PriceSnapshot>> GetLatestPerStockAsync(CancellationToken ct = default)
        {
            return await _db.PriceSnapshots
                .GroupBy(p => p.StockId)
                .Select(g => g.OrderByDescending(p => p.FetchedAtUtc).First())
                .ToListAsync(ct);
        }
    }
}