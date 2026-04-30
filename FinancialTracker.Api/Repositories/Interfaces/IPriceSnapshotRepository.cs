using FinancialTracker.Api.Models;

namespace FinancialTracker.Api.Repositories.Interfaces
{
    public interface IPriceSnapshotRepository
    {
        Task AddAsync(PriceSnapshot snapshot, CancellationToken ct = default);
        Task<List<PriceSnapshot>> GetLatestPerStockAsync(CancellationToken ct = default);
    }
}