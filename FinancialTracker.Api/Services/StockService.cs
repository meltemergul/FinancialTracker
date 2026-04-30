using FinancialTracker.Api.Clients.Interfaces;
using FinancialTracker.Api.DTOs.Responses;
using FinancialTracker.Api.Models;
using FinancialTracker.Api.Repositories.Interfaces;
using FinancialTracker.Api.Services.Interfaces;

namespace FinancialTracker.Api.Services
{

    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IPriceSnapshotRepository _snapshotRepository;
        private readonly IFinanceApiClient _financeApiClient;
        public StockService(
            IStockRepository stockRepository,
            IPriceSnapshotRepository snapshotRepository,
            IFinanceApiClient financeApiClient)
        {
            _stockRepository = stockRepository;
            _snapshotRepository = snapshotRepository;
            _financeApiClient = financeApiClient;
        }
        public async Task<StockDto> AddStockAsync(string symbol, CancellationToken ct = default)
        {
            symbol = symbol.Trim().ToUpperInvariant();
            var existing = await _stockRepository.GetBySymbolAsync(symbol, ct);
            if (existing is not null)
                throw new ArgumentException($"Stock {symbol} already exists.");
            var (current, previousClose) = await _financeApiClient.GetQuoteAsync(symbol, ct);
            var stock = new Stock
            {
                Symbol = symbol,
                CompanyName = symbol
            };
            await _stockRepository.AddAsync(stock, ct);
            await _stockRepository.SaveChangesAsync(ct);
            var snapshot = new PriceSnapshot
            {
                StockId = stock.Id,
                CurrentPrice = current,
                PreviousClose = previousClose,
                FetchedAtUtc = DateTime.UtcNow
            };
            await _snapshotRepository.AddAsync(snapshot, ct);
            await _stockRepository.SaveChangesAsync(ct);
            return new StockDto
            {
                Symbol = stock.Symbol,
                CompanyName = stock.CompanyName,
                LatestPrice = snapshot.CurrentPrice,
                LastFetchedAtUtc = snapshot.FetchedAtUtc
            };
        }
        public async Task<List<StockDto>> GetAllAsync(CancellationToken ct = default)
        {
            var stocks = await _stockRepository.GetAllWithSnapshotsAsync(ct);
            return stocks.Select(s =>
            {
                var latest = s.PriceSnapshots.OrderByDescending(p => p.FetchedAtUtc).FirstOrDefault();
                return new StockDto
                {
                    Symbol = s.Symbol,
                    CompanyName = s.CompanyName,
                    LatestPrice = latest?.CurrentPrice,
                    LastFetchedAtUtc = latest?.FetchedAtUtc
                };
            }).ToList();
        }
        public async Task RefreshAsync(string symbol, CancellationToken ct = default)
        {
            symbol = symbol.Trim().ToUpperInvariant();
            var stock = await _stockRepository.GetBySymbolAsync(symbol, ct)
                ?? throw new KeyNotFoundException($"Stock {symbol} was not found.");
            var (current, previousClose) = await _financeApiClient.GetQuoteAsync(symbol, ct);
            var snapshot = new PriceSnapshot
            {
                StockId = stock.Id,
                CurrentPrice = current,
                PreviousClose = previousClose,
                FetchedAtUtc = DateTime.UtcNow
            };
            await _snapshotRepository.AddAsync(snapshot, ct);
            await _stockRepository.SaveChangesAsync(ct);
        }
        public async Task DeleteAsync(string symbol, CancellationToken ct = default)
        {
            symbol = symbol.Trim().ToUpperInvariant();
            var stock = await _stockRepository.GetBySymbolAsync(symbol, ct)
                ?? throw new KeyNotFoundException($"Stock {symbol} was not found.");
            await _stockRepository.DeleteAsync(stock, ct);
            await _stockRepository.SaveChangesAsync(ct);
        }
    }
}