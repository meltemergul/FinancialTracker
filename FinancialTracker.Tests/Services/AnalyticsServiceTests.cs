using FinancialTracker.Api.Models;
using FinancialTracker.Api.Repositories.Interfaces;
using FinancialTracker.Api.Services;
using Moq;

namespace FinancialTracker.Tests.Services;

public class AnalyticsServiceTests
{
    [Fact]
    public async Task GetTopGrowthAsync_ReturnsSortedTopN_AndExcludesInvalidPreviousClose()
    {
        // Arrange
        var stockRepository = new Mock<IStockRepository>();
        stockRepository
            .Setup(r => r.GetAllWithSnapshotsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Stock>
            {
                new()
                {
                    Symbol = "AAPL",
                    PriceSnapshots =
                    [
                        new PriceSnapshot { CurrentPrice = 110m, PreviousClose = 100m, Stock = new Stock { Symbol = "AAPL" } }
                    ]
                },
                new()
                {
                    Symbol = "MSFT",
                    PriceSnapshots =
                    [
                        new PriceSnapshot { CurrentPrice = 125m, PreviousClose = 100m, Stock = new Stock { Symbol = "MSFT" } }
                    ]
                },
                new()
                {
                    Symbol = "TSLA",
                    PriceSnapshots =
                    [
                        new PriceSnapshot { CurrentPrice = 95m, PreviousClose = 100m, Stock = new Stock { Symbol = "TSLA" } }
                    ]
                },
                new()
                {
                    Symbol = "NVDA",
                    PriceSnapshots =
                    [
                        new PriceSnapshot { CurrentPrice = 300m, PreviousClose = 0m, Stock = new Stock { Symbol = "NVDA" } }
                    ]
                }
            });

        var service = new AnalyticsService(stockRepository.Object);

        // Act
        var results = await service.GetTopGrowthAsync(2, CancellationToken.None);

        // Assert
        Assert.Equal(2, results.Count);
        Assert.Equal("MSFT", results[0].Symbol);
        Assert.Equal("AAPL", results[1].Symbol);
        Assert.DoesNotContain(results, r => r.Symbol == "NVDA");
    }
}