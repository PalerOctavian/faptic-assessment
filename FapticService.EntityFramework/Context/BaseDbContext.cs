using FapticService.Domain.Models;
using FapticService.EntityFramework.EntitiesConfiguration;
using Microsoft.EntityFrameworkCore;

namespace FapticService.EntityFramework.Context;

public class BaseDbContext : DbContext
{
    public DbSet<PricePoint> PricePoints { get; set; }

    public BaseDbContext(DbContextOptions<BaseDbContext> options)
        : base(options) { }
    
    protected BaseDbContext(DbContextOptions options) 
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PricePointConfig());
        
        base.OnModelCreating(modelBuilder);
    }
}