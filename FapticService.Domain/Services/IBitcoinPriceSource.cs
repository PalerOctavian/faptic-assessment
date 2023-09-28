namespace FapticService.Domain.Services;

public interface IBitcoinPriceSource
{
    /// <summary>
    ///     Fetch bitcoin price from 3rd party
    /// </summary>
    /// <param name="timestamp">UTC timestamp</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int?> FetchBitcoinPrice(DateTime timestamp, CancellationToken cancellationToken = default);
}
