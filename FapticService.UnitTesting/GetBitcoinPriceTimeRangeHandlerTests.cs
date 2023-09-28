using AutoFixture;
using AutoMapper;
using FapticService.API.V1.Dto;
using FapticService.API.V1.Requests;
using FapticService.Business.Queries;
using FapticService.Domain.Models;
using FapticService.Domain.Repository;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FapticService.UnitTesting;

public class GetBitcoinPriceTimeRangeHandlerTests : UnitTestBase<GetBitcoinPriceTimeRangeHandler>
{
    private readonly Mock<IPricePointReadOnlyRepository> repositoryMock;
    private readonly Mock<IMapper> mapperMock;
    
    public GetBitcoinPriceTimeRangeHandlerTests()
    {
        repositoryMock = Fixture.Freeze<Mock<IPricePointReadOnlyRepository>>();
        mapperMock = Fixture.Freeze<Mock<IMapper>>();

        Fixture.Inject(Mock.Of<ILogger<GetBitcoinPriceTimeRangeHandler>>());
    }

    [Fact]
    public async Task Get_By_Time_Range_Test()
    {
        // Arrange
        var pricePoints = new List<PricePoint>
        {
            new() {Id = 1, Price = 200, SourcesFetched = 2, Timestamp = DateTime.UtcNow}
        };
        var pricePointsDto = new List<PricePointDto>
        {
            new() {Price = 200, Timestamp = DateTime.UtcNow}
        };
        repositoryMock
            .Setup(repo => repo.GetByTimestamps(It.IsAny<DateTime>(), It.IsAny<DateTime>(),It.IsAny<CancellationToken>()))
            .ReturnsAsync(pricePoints);
        mapperMock.Setup(mapper => mapper.Map<IEnumerable<PricePointDto>>(It.IsAny<IEnumerable<PricePoint>>()))
            .Returns(pricePointsDto);
        
        // Act
        var result = await Sut.Handle(new GetBitcoinPriceTimeRangeRequest(), new CancellationToken());
        
        // Assert
        repositoryMock.Verify(repo => repo.GetByTimestamps(It.IsAny<DateTime>(), It.IsAny<DateTime>(),It.IsAny<CancellationToken>()),
            Times.Once);
        mapperMock.Verify(mapper => mapper.Map<IEnumerable<PricePointDto>>(It.IsAny<IEnumerable<PricePoint>>()),
            Times.Once);
        result.Should().NotBeNull();
        result.Prices.Count().Should().Be(1);
    }
}
