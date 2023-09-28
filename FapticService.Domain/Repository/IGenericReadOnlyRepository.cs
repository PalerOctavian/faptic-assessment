using System.Linq.Expressions;

namespace FapticService.Domain.Repository;

public interface IGenericReadOnlyRepository<T>
{
    Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, bool>>? filter = null,
        CancellationToken cancellationToken = default);
}