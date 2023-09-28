using AutoFixture;
using FapticService.Business.Services;
using FapticService.Domain.Models;
using FapticService.Domain.Repository;
using FapticService.Domain.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FapticService.UnitTesting;

public class BitcoinPriceServiceTests : UnitTestBase<BitcoinPriceService>
{
    private readonly Mock<IPricePointRepository> repositoryMock;
    private readonly Mock<IBitcoinPriceSource> bitcoinPriceSourceMock;

    public BitcoinPriceServiceTests() : base()
    {
        repositoryMock = Fixture.Freeze<Mock<IPricePointRepository>>();
        bitcoinPriceSourceMock = Fixture.Freeze<Mock<IBitcoinPriceSource>>();

        var bitcoinPriceSources = new[]
        {
            bitcoinPriceSourceMock.Object
        };
        
        Fixture.Inject(Mock.Of<ILogger<BitcoinPriceService>>());
        Fixture.Inject<IEnumerable<IBitcoinPriceSource>>(bitcoinPriceSources);
    }

    [Fact]
    public async Task Fetch_From_Source_And_Store_Test()
    {
        // Arrange
        repositoryMock
            .Setup(repo => repo.GetByTimestamp(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((PricePoint?)null);
        repositoryMock
            .Setup(repo => repo.CreateAsync(It.IsAny<PricePoint>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PricePoint());
        bitcoinPriceSourceMock
            .Setup(service => service.FetchBitcoinPrice(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(200);
        
        // Act
        var result = await Sut.FetchAndAggregate(DateTime.UtcNow);
        
        // Assert
        repositoryMock.Verify(repo => repo.GetByTimestamp(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()),
            Times.Once);
        repositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<PricePoint>(), It.IsAny<CancellationToken>()),
            Times.Once);
        bitcoinPriceSourceMock.Verify(service => service.FetchBitcoinPrice(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()),
            Times.Once);
        result.Should().NotBeNull();
        result.sourcesFetched.Should().Be(1);
        result.price.Should().Be(200);
    }

    [Fact]
    public async Task Fetch_And_Return_From_Database_Test()
    {
        // Arrange
        repositoryMock
            .Setup(repo => repo.GetByTimestamp(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PricePoint{Price = 200, SourcesFetched = 2});
        repositoryMock
            .Setup(repo => repo.CreateAsync(It.IsAny<PricePoint>(), It.IsAny<CancellationToken>()));
        bitcoinPriceSourceMock
            .Setup(service => service.FetchBitcoinPrice(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()));
        
        // Act
        var result = await Sut.FetchAndAggregate(DateTime.UtcNow);
        
        // Assert
        repositoryMock.Verify(repo => repo.GetByTimestamp(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()),
            Times.Once);
        repositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<PricePoint>(), It.IsAny<CancellationToken>()),
            Times.Never);
        bitcoinPriceSourceMock.Verify(service => service.FetchBitcoinPrice(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()),
            Times.Never);
        result.Should().NotBeNull();
        result.sourcesFetched.Should().Be(2);
        result.price.Should().Be(200);
    }
}
