using ConfigurationSubstitution;
using FapticService.Configuration;
using FapticService.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config
            .AddEnvironmentVariables()
            .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true,
                reloadOnChange: true)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            // Enable passing variables as secrets and replace them with env vars. Format %VARIABLE_NAME%
            .EnableSubstitutions("%", "%");
    })
    .ConfigureServices((hostBuilderContext, serviceCollection) =>
    {
        serviceCollection.AddDependencies(hostBuilderContext.Configuration);
    })
    .UseSerilog((hostingContext, loggerConfig) => loggerConfig.ReadFrom.Configuration(hostingContext.Configuration));

var app = builder.Build().UseMiddlewares();

// Apply db migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ServiceDbContext>();    
    context.Database.Migrate();
}

app.Run();
