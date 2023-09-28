using FapticService.Common.Constants;
using FapticService.Common.Extensions;
using FapticService.Domain.Repository;
using FapticService.EntityFramework.Context;
using FapticService.EntityFramework.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FapticService.EntityFramework;

public static class EntityFrameworkConfiguration
{
    public static void AddEntityFramework(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<ServiceDbContext>(options =>
            options.UseNpgsql(configuration.GetDatabaseConnectionString()));
        serviceCollection.AddDbContext<ServiceReadOnlyDbContext>(options =>
            options.UseNpgsql(configuration.GetDatabaseConnectionString()));

        serviceCollection.AddScoped<IPricePointRepository, PricePointRepository>();
        serviceCollection.AddScoped<IPricePointReadOnlyRepository, PricePointRepository>();
    }
}
