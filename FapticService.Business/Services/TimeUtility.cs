using FapticService.Business.Contract;

namespace FapticService.Business.Services;

// Using a class instead of extension method for testability
public class TimeUtility : ITimeUtility
{
    public DateTime ToHourPrecision(DateTime timestamp)
    {
        return new DateTime(timestamp.Year, timestamp.Month, timestamp.Day, timestamp.Hour, 0, 0, DateTimeKind.Utc);
    }
}
