using FapticService.Common.Constants;
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
            options.UseNpgsql(GetConnectionString(configuration)));
        serviceCollection.AddDbContext<ServiceReadOnlyDbContext>(options =>
            options.UseNpgsql(GetConnectionString(configuration)));

        serviceCollection.AddScoped<IPricePointRepository, PricePointRepository>();
        serviceCollection.AddScoped<IPricePointReadOnlyRepository, PricePointRepository>();
    }
    
    private static string GetConnectionString(IConfiguration configuration)
    {
        var host = configuration[ConfigurationConstants.DatabaseHost];
        var port = configuration[ConfigurationConstants.DatabasePort];
        var user = configuration[ConfigurationConstants.DatabaseUser];
        var password = configuration[ConfigurationConstants.DatabasePassword];
        var database = configuration[ConfigurationConstants.DatabaseName];
        
        return $"Host={host};port={port};Username={user};Password={password};Database={database};Pooling=true";
    }
}