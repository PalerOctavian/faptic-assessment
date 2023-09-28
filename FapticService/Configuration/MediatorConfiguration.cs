using FapticService.API.V1.Requests;
using FapticService.Business.Queries;

namespace FapticService.Configuration;

public static class MediatorConfiguration
{
    public static void AddMediator(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<FetchBitcoinPriceRequest>();
            cfg.RegisterServicesFromAssemblyContaining<FetchBitcoinPriceHandler>();
        });
    }
}
