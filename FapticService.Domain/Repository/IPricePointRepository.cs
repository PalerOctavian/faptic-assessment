using FapticService.Domain.Models;

namespace FapticService.Domain.Repository;

/// <summary>
///     Interface for PricePoint repository DI
/// </summary>
public interface IPricePointRepository : IPricePointReadOnlyRepository, IGenericRepository<PricePoint>
{
    
}