using FapticService.Domain.Repository;
using FapticService.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace FapticService.EntityFramework.Repository.Generic;

public class GenericRepository<T> : GenericReadOnlyRepository<T>, IGenericRepository<T> where T : class
{
    protected GenericRepository(BaseDbContext context) : base(context) { }
    
    public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await Context.Set<T>().AddAsync(entity, cancellationToken);

        await Context.SaveChangesAsync(cancellationToken);
        
        return entity;
    }
    
    public void Update(T entity, string modifiedBy = "")
    {
        Context.Set<T>().Attach(entity);
        Context.Entry(entity).State = EntityState.Modified;
    }
    
    public async Task Delete(T entity, CancellationToken cancellationToken = default)
    {
        var dbSet = Context.Set<T>();
        dbSet.Remove(entity);

        await Context.SaveChangesAsync(cancellationToken);
    }
}