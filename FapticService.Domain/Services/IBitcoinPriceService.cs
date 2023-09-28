namespace FapticService.Domain.Services;

public interface IBitcoinPriceService
{
    /// <summary>
    ///     Fetch bitcoin prices and aggregate the values
    /// </summary>
    /// <param name="timestamp">UTC Time</param>
    /// <param name="cancellationToken">cancellation token</param>
    /// <returns></returns>
    Task<(double price, int sourcesFetched)> FetchAndAggregate(DateTime timestamp, CancellationToken cancellationToken = default);
}