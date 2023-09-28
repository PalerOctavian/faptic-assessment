using FapticService.Business.Services;
using FluentAssertions;
using Xunit;

namespace FapticService.UnitTesting;

public class TimeUtilityTests : UnitTestBase<TimeUtility>
{
    [Fact]
    public void Time_To_Hour_Precision()
    {
        // Arrange
        var time = DateTime.UtcNow;
        
        // Act
        var result = Sut.ToHourPrecision(time);

        // Assert
        result.Minute.Should().Be(0);
        result.Second.Should().Be(0);
        result.Millisecond.Should().Be(0);
    }
}
