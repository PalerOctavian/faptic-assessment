namespace FapticService.Common.Extensions;

public static class DateTimeExtensions
{
    public static long ToUnixTimeSeconds(this DateTime dateTime)
    {
        var unixTimestamp = ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
        return unixTimestamp;
    }
    
    public static long ToUnixTimeMilliseconds(this DateTime dateTime)
    {
        var unixTimestamp = ((DateTimeOffset)dateTime).ToUnixTimeMilliseconds();
        return unixTimestamp;
    }
}
