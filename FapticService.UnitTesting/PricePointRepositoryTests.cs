using AutoFixture;
using FapticService.Domain.Models;
using FapticService.EntityFramework.Context;
using FapticService.EntityFramework.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FapticService.UnitTesting;

public class PricePointRepositoryTests : UnitTestBase<PricePointRepository>, IDisposable
{
    private readonly ServiceDbContext context;

    public PricePointRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ServiceDbContext>()
            .UseInMemoryDatabase(databaseName: $"faptic-service-{Guid.NewGuid()}")
            .Options;

        context = new ServiceDbContext(options);
        Fixture.Inject(context);
    }

    [Fact]
    public async Task When_Get_Timestamp_Entity_Returns()
    {
        // Arrange
        var time = DateTime.UtcNow;
        await context.PricePoints.AddAsync(new PricePoint
        {
            Price = 200, SourcesFetched = 2, Timestamp = time
        });
        await context.SaveChangesAsync();
        
        // Act
        var result = await Sut.GetByTimestamp(time);

        // Assert
        result.Should().NotBeNull();
    }
    
    [Fact]
    public async Task When_Get_Timestamps_Correct_Entities_Returned()
    {
        // Arrange
        var time = DateTime.UtcNow;
        await context.PricePoints.AddAsync(
            new PricePoint {Price = 200, SourcesFetched = 2, Timestamp = time
        });
        await context.PricePoints.AddAsync(
            new PricePoint {Price = 210, SourcesFetched = 2, Timestamp = time.AddDays(-1)
            });
        await context.PricePoints.AddAsync(
            new PricePoint {Price = 205, SourcesFetched = 2, Timestamp = time. AddDays(-2)
            });
        await context.SaveChangesAsync();
        
        // Act
        var result = await Sut.GetByTimestamps(time.AddDays(-3), time.AddMinutes(-1));

        // Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(2);
    }
    
    public void Dispose()
    {
        context.Dispose();
    }
}
