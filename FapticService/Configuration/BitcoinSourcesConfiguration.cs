using FapticService.Business.ApiClients;
using FapticService.Domain.Services;

namespace FapticService.Configuration;

public static class BitcoinSourcesConfiguration
{
    public static void AddBitcoinSources(this IServiceCollection serviceCollection)
    {
        AddBitstampSource(serviceCollection);
        AddBitfinexSource(serviceCollection);
    }

    private static void AddBitstampSource(IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient("bitstamp", client =>
        {
            client.BaseAddress = new Uri("https://www.bitstamp.net");
        });

        serviceCollection.AddTransient<IBitcoinPriceSource, BitstampApiClient>();
    }

    private static void AddBitfinexSource(IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient("bitfinex", client =>
        {
            client.BaseAddress = new Uri("https://api-pub.bitfinex.com");
        });

        serviceCollection.AddTransient<IBitcoinPriceSource, BitfinexApiClient>();
    }
}