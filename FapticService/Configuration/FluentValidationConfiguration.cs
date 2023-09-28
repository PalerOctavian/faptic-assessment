using FapticService.API.V1.Requests;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace FapticService.Configuration;

public static class FluentValidationConfiguration
{
    public static void AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<FetchBitcoinPriceRequestValidator>();
        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();
    }
}