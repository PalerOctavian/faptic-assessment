using Microsoft.EntityFrameworkCore;

namespace FapticService.EntityFramework.Context;

public class ServiceReadOnlyDbContext : BaseDbContext
{
    public ServiceReadOnlyDbContext(DbContextOptions<ServiceReadOnlyDbContext> options) : base(options)
    {
    }
}