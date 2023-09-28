using System.Linq.Expressions;
using FapticService.Domain.Repository;
using FapticService.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace FapticService.EntityFramework.Repository.Generic;

public class GenericReadOnlyRepository<T> : IGenericReadOnlyRepository<T> where T : class
{
    protected readonly BaseDbContext Context;
    
    protected GenericReadOnlyRepository(BaseDbContext context)
    {
        Context = context;
    }
    
    public Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, bool>>? filter = null,
        CancellationToken cancellationToken = default)
    {
        return GetQueryable(filter).FirstOrDefaultAsync(cancellationToken);
    }
    
    protected IQueryable<T> GetQueryable(
        Expression<Func<T, bool>>? filter = null,
        int? skip = null,
        int? take = null)
    {
        IQueryable<T> query = Context.Set<T>();

        if (filter != null)
        {
            query = query.Where(filter);
        }
          
        if (skip.HasValue)
        {
            query = query.Skip(skip.Value);
        }

        if (take.HasValue)
        {
            query = query.Take(take.Value);
        }

        return query;
    }
}