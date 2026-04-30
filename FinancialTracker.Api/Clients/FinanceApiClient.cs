using System.Text.Json;
using FinancialTracker.Api.Clients.Interfaces;

namespace FinancialTracker.Api.Clients
{
    public class FinnhubApiClient : IFinanceApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public FinnhubApiClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<(decimal CurrentPrice, decimal PreviousClose)> GetQuoteAsync(string symbol, CancellationToken ct = default)
        {
            var apiKey = _configuration["Finnhub:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("Finnhub API key is not configured.");
            var url = $"quote?symbol={symbol}&token={apiKey}";
            using var response = await _httpClient.GetAsync(url, ct);
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Finnhub request failed with status {response.StatusCode}.");
            var json = await response.Content.ReadAsStringAsync(ct);
            using var doc = JsonDocument.Parse(json);

            var root = doc.RootElement;
            var current = root.GetProperty("c").GetDecimal();
            var prevClose = root.GetProperty("pc").GetDecimal();
            if (current <= 0 || prevClose < 0)
                throw new InvalidOperationException("Finnhub returned invalid quote data.");
            return (current, prevClose);
        }
    }
}