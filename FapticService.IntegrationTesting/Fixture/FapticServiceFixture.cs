using FapticService.EntityFramework.Context;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Xunit;

namespace FapticService.IntegrationTesting.Fixture;

[CollectionDefinition(nameof(FapticServiceFixture))]
public class FapticServiceCollectionFixture : ICollectionFixture<FapticServiceFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

public class FapticServiceFixture
{
    private readonly HttpClient client;
    private readonly TestServer server;
    
    public HttpClient Client
    {
        get
        {
            if (client == null)
            {
                throw new InvalidOperationException();
            }

            return client;
        }
    }
    
    public TestServer Server
    {
        get
        {
            if (server == null)
            {
                throw new InvalidOperationException();
            }
            return server;
        }
    }

    public FapticServiceFixture()
    {
        var application = new FapticServiceTestApplication();
        server = application.Server;
        client = application.Server.CreateClient();
        
        EnsureDatabase();
    }
    
    private void EnsureDatabase()
    {
        DbContext dbContext = server.Services.GetRequiredService<ServiceDbContext>();

        DropDb(dbContext);
        CreateDb(dbContext);
    }
    
    
    private static void CreateDb(DbContext context)
    {
        // using EF migration
        // migrate any database changes on startup (includes initial db creation)
        try
        {
            var conn = context.Database.GetDbConnection().ConnectionString;
            context.Database.Migrate();
        }
        catch (PostgresException)
        {
            throw;
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    private static void DropDb(DbContext context)
    {
        context.Database.EnsureDeleted();
    }
}
