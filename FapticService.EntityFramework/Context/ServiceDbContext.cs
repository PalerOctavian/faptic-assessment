using Microsoft.EntityFrameworkCore;

namespace FapticService.EntityFramework.Context;

public class ServiceDbContext : BaseDbContext
{
    public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options)
    {
    }
}