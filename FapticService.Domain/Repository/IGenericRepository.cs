namespace FapticService.Domain.Repository;

public interface IGenericRepository<T> : IGenericReadOnlyRepository<T>
{
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
    
    void Update(T entity, string modifiedBy = "");
    
    Task Delete(T entity, CancellationToken cancellationToken = default);
}