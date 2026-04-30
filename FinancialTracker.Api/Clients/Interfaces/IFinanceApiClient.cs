namespace FinancialTracker.Api.Clients.Interfaces
{
    public interface IFinanceApiClient
    {
        Task<(decimal CurrentPrice, decimal PreviousClose)> GetQuoteAsync(string symbol, CancellationToken ct = default);
    }
}
