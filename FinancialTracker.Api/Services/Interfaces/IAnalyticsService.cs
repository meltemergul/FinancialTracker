using FinancialTracker.Api.DTOs.Responses;

namespace FinancialTracker.Api.Services.Interfaces
{

    public interface IAnalyticsService
    {
        Task<List<GrowthResultDto>> GetTopGrowthAsync(int n, CancellationToken ct = default);
    }
}