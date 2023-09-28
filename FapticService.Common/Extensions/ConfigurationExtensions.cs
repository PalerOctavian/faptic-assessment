using FapticService.Common.Constants;
using Microsoft.Extensions.Configuration;

namespace FapticService.Common.Extensions;

public static class ConfigurationExtensions
{
    public static string GetDatabaseConnectionString(this IConfiguration configuration)
    {
        var host = configuration[ConfigurationConstants.DatabaseHost];
        var port = configuration[ConfigurationConstants.DatabasePort];
        var user = configuration[ConfigurationConstants.DatabaseUser];
        var password = configuration[ConfigurationConstants.DatabasePassword];
        var database = configuration[ConfigurationConstants.DatabaseName];
        
        return $"Host={host};port={port};Username={user};Password={password};Database={database};Pooling=true";
    }
}
