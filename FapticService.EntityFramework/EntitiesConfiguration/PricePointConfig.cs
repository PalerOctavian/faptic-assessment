using FapticService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FapticService.EntityFramework.EntitiesConfiguration;

public class PricePointConfig : IEntityTypeConfiguration<PricePoint>
{
    public void Configure(EntityTypeBuilder<PricePoint> builder)
    {
        builder.ToTable(nameof(PricePoint));
        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.Price).IsRequired();
        builder.Property(entity => entity.SourcesFetched).IsRequired();
        builder.Property(entity => entity.Timestamp).IsRequired().HasColumnType("timestamp with time zone");
    }
}