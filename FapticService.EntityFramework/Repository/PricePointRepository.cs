using FapticService.Domain.Models;
using FapticService.Domain.Repository;
using FapticService.EntityFramework.Context;
using FapticService.EntityFramework.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace FapticService.EntityFramework.Repository;

public class PricePointRepository : GenericRepository<PricePoint>, IPricePointRepository
{
    public PricePointRepository(ServiceDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PricePoint>> GetByTimestamps(DateTime startUtc, DateTime endUtc, CancellationToken cancellationToken = default)
    {
        return await GetQueryable(entity => 
            entity.Timestamp >= DateTime.SpecifyKind(startUtc, DateTimeKind.Utc) && 
            entity.Timestamp <= DateTime.SpecifyKind(endUtc, DateTimeKind.Utc)).ToListAsync(cancellationToken);
    }

    public Task<PricePoint?> GetByTimestamp(DateTime timestampUtc, CancellationToken cancellationToken = default)
    {
        return GetFirstOrDefaultAsync(entity => entity.Timestamp.Equals(timestampUtc), cancellationToken);
    }
}