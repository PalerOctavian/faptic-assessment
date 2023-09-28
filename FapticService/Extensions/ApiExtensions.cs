namespace FapticService.Extensions;

public static class ApiExtensions
{
    private const string LocalEnvironment = "local";
    
    public static bool IsDevelopmentOrLocal(this IWebHostEnvironment environment)
    {
        return environment.IsDevelopment() || environment.IsEnvironment(LocalEnvironment);
    }
}
