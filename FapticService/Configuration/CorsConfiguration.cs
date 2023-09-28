namespace FapticService.Configuration;

public static class CorsConfiguration
{
    public const string DefaultCorsPolicy = "_defaultCorsPolicy";

    public static void AddCorsConfiguration(this IServiceCollection serviceCollection)
    {

        serviceCollection.AddCors(options =>
        {
            options.AddPolicy(name: DefaultCorsPolicy,
                policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
    }
}
