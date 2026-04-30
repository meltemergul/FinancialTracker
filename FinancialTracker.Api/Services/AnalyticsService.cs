using FinancialTracker.Api.DTOs.Responses;
using FinancialTracker.Api.Repositories.Interfaces;
using FinancialTracker.Api.Services.Interfaces;


namespace FinancialTracker.Api.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IStockRepository _stockRepository;
        public AnalyticsService(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }
        public async Task<List<GrowthResultDto>> GetTopGrowthAsync(int n, CancellationToken ct = default)
        {
            if (n <= 0) n = 5;
            var stocks = await _stockRepository.GetAllWithSnapshotsAsync(ct);
            var results = stocks
                .Select(s => s.PriceSnapshots.OrderByDescending(p => p.FetchedAtUtc).FirstOrDefault())
                .Where(p => p is not null && p.PreviousClose > 0)
                .Select(p => new GrowthResultDto
                {
                    Symbol = p!.Stock.Symbol,
                    CurrentPrice = p.CurrentPrice,
                    PreviousClose = p.PreviousClose,
                    GrowthPercent = Math.Round(((p.CurrentPrice - p.PreviousClose) / p.PreviousClose) * 100, 2)
                })
                .OrderByDescending(x => x.GrowthPercent)
                .Take(n)
                .ToList();
            return results;
        }
    }
}