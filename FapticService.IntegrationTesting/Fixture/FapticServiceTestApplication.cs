using FapticService.Common.Extensions;
using FapticService.EntityFramework.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace FapticService.IntegrationTesting.Fixture;

public class FapticServiceTestApplication : WebApplicationFactory<Program>
{
    private const string TestEnvironment = "local";

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureWebHost(webHostBuilder =>
        {
            webHostBuilder.UseTestServer()
                .UseEnvironment(TestEnvironment);
        });
        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices((hostBuilderContext, serviceCollection) =>
        {
            var connectionString = hostBuilderContext.Configuration.GetDatabaseConnectionString();
            
            var npgsqlConnectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);
            // Generate unique name for database
            npgsqlConnectionStringBuilder.Database = $"{npgsqlConnectionStringBuilder.Database}_{Guid.NewGuid()}";
            var connection = npgsqlConnectionStringBuilder.ConnectionString;
            
            var dbContext = serviceCollection.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ServiceDbContext>));
            var readOnlyDbContext = serviceCollection.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ServiceReadOnlyDbContext>));
            
            RemoveService(serviceCollection, dbContext);
            RemoveService(serviceCollection, readOnlyDbContext);
            
            serviceCollection.AddDbContext<ServiceDbContext>(options => options.UseNpgsql(connection));
            serviceCollection.AddDbContext<ServiceReadOnlyDbContext>(options => options.UseNpgsql(connection));

        });
        base.ConfigureWebHost(builder);
    }
    
    private static void RemoveService(IServiceCollection collection, ServiceDescriptor? dbContext)
    {
        if (dbContext != null)
        {
            collection.Remove(dbContext);
        }
    }
}
