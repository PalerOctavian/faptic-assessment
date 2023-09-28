using AutoFixture;
using FapticService.API.V1.Requests;
using FapticService.Business.Contract;
using FapticService.Business.Queries;
using FapticService.Domain.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FapticService.UnitTesting;

public class FetchBitcoinPriceHandlerTests : UnitTestBase<FetchBitcoinPriceHandler>
{
    private readonly Mock<IBitcoinPriceService> bitcoinPriceServiceMock;
    private readonly Mock<ITimeUtility> timeUtilityMock;

    public FetchBitcoinPriceHandlerTests()
    {
        bitcoinPriceServiceMock = Fixture.Freeze<Mock<IBitcoinPriceService>>();
        timeUtilityMock = Fixture.Freeze<Mock<ITimeUtility>>();

        Fixture.Inject(Mock.Of<ILogger<FetchBitcoinPriceHandler>>());
    }
    
    [Fact]
    public async Task Fetch_Price_Test()
    {
        // Arrange
        bitcoinPriceServiceMock
            .Setup(repo => repo.FetchAndAggregate(It.IsAny<DateTime>(),It.IsAny<CancellationToken>()))
            .ReturnsAsync((200, 2));
        timeUtilityMock.Setup(service => service.ToHourPrecision(It.IsAny<DateTime>()))
            .Returns(DateTime.UtcNow);

        // Act
        var result = await Sut.Handle(new FetchBitcoinPriceRequest{ UtcTime = DateTime.UtcNow}, new CancellationToken());
        
        // Assert
        bitcoinPriceServiceMock.Verify(
            service => service.FetchAndAggregate(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()),
            Times.Once);
        timeUtilityMock.Verify(service => service.ToHourPrecision(It.IsAny<DateTime>()),
            Times.Once);

        result.Should().NotBeNull();
        result.Price.Should().Be(200);
        result.SourceFetched.Should().Be(2);
    }
}
