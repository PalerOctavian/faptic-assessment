using FapticService.Business.Contract;
using FapticService.Business.Services;
using FapticService.Domain.Services;
using FapticService.EntityFramework;
using FapticService.Extensions;
using FapticService.Filters;

namespace FapticService.Configuration;

public static class ServiceConfiguration
{
    public static void AddDependencies(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddControllers(c =>
        {
            c.Filters.Add(typeof(GlobalExceptionFilter));
        });
        serviceCollection.AddVersioning();
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddCorsConfiguration();
        serviceCollection.AddMediator();
        serviceCollection.AddFluentValidation();
        serviceCollection.AddAutoMapping();

        serviceCollection.AddEntityFramework(configuration);

        serviceCollection.AddTransient<IBitcoinPriceService, BitcoinPriceService>();
        serviceCollection.AddSingleton<ITimeUtility, TimeUtility>();

        serviceCollection.AddBitcoinSources();
    }

    public static WebApplication UseMiddlewares(this WebApplication app)
    {
        if (app.Environment.IsDevelopmentOrLocal())
        {
            app.UseSwagger(setup =>{
                setup.RouteTemplate = "faptic-service/swagger/{documentName}/swagger.json";
            });
            app.UseCors(CorsConfiguration.DefaultCorsPolicy);
            app.UseSwaggerUI(setup =>
            {
                setup.SwaggerEndpoint($"/faptic-service/swagger/v1/swagger.json", "Faptic Service v1");
                setup.RoutePrefix = "faptic-service/swagger";
            });
        }
        
        app.UseHttpsRedirection();

        // Enable authentication and authorization (for this exercise there is no identity provider setup)
        // app.UseAuthentication();
        // app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
