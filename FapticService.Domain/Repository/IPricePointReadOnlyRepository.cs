using FapticService.Domain.Models;

namespace FapticService.Domain.Repository;

public interface IPricePointReadOnlyRepository : IGenericReadOnlyRepository<PricePoint>
{
    Task<IEnumerable<PricePoint>> GetByTimestamps(
        DateTime startUtc,
        DateTime endUtc,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get price point by utc timestamp with hour precision
    /// </summary>
    /// <param name="timestampUtc"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PricePoint?> GetByTimestamp(DateTime timestampUtc, CancellationToken cancellationToken = default);
}