using FinancialTracker.Api.Clients.Interfaces;
using FinancialTracker.Api.Repositories.Interfaces;
using FinancialTracker.Api.Services;
using Moq;

namespace FinancialTracker.Tests.Services;

public class StockServiceTests
{
    [Fact]
    public async Task AddStockAsync_WhenSymbolAlreadyExists_ThrowsArgumentException()
    {
        // Arrange
        var stockRepository = new Mock<IStockRepository>();
        var snapshotRepository = new Mock<IPriceSnapshotRepository>();
        var financeApiClient = new Mock<IFinanceApiClient>();

        stockRepository
            .Setup(r => r.GetBySymbolAsync("AAPL", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FinancialTracker.Api.Models.Stock { Symbol = "AAPL" });

        var service = new StockService(
            stockRepository.Object,
            snapshotRepository.Object,
            financeApiClient.Object);

        // Act
        var act = () => service.AddStockAsync("aapl", CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(act);

        financeApiClient.Verify(
            c => c.GetQuoteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}