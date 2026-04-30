using FinancialTracker.Api.DTOs.Responses;
namespace FinancialTracker.Api.Services.Interfaces
{

    public interface IStockService
    {
        Task<StockDto> AddStockAsync(string symbol, CancellationToken ct = default);
        Task<List<StockDto>> GetAllAsync(CancellationToken ct = default);
        Task RefreshAsync(string symbol, CancellationToken ct = default);
        Task DeleteAsync(string symbol, CancellationToken ct = default);
    }
}