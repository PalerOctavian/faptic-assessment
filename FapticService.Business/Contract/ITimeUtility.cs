namespace FapticService.Business.Contract;

public interface ITimeUtility
{
    DateTime ToHourPrecision(DateTime timestamp);
}
