using FinancialTracker.Api.Models;
namespace FinancialTracker.Api.Repositories.Interfaces
{
    public interface IStockRepository
    {
        Task<Stock?> GetBySymbolAsync(string symbol, CancellationToken ct = default);
        Task<List<Stock>> GetAllWithSnapshotsAsync(CancellationToken ct = default);
        Task AddAsync(Stock stock, CancellationToken ct = default);
        Task DeleteAsync(Stock stock, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}