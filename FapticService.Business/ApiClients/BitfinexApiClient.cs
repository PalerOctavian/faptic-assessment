using FapticService.Common.Extensions;
using FapticService.Domain.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace FapticService.Business.ApiClients;

public class BitfinexApiClient : IBitcoinPriceSource
{
    private const int HOUR_IN_MILLISECONDS = 3_600_000;
    private const string BITCOIN_PRICE_ROUTE = "v2/candles/trade:1h:tBTCUSD/hist?start={0}&end={1}&limit=1";

    private readonly ILogger<BitfinexApiClient> logger;
    private readonly HttpClient httpClient;

    public BitfinexApiClient(
        ILogger<BitfinexApiClient> logger,
        IHttpClientFactory httpClientFactory)
    {
        this.logger = logger;
        httpClient = httpClientFactory.CreateClient("bitfinex");
    }

    public async Task<int?> FetchBitcoinPrice(DateTime timestamp, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching bitstamp for bitcoin price");

        var startUnixTimestamp = timestamp.ToUnixTimeMilliseconds();
        var endUnixTimestamp = startUnixTimestamp + HOUR_IN_MILLISECONDS;
        
        JArray result;
        try
        {
            var x = string.Format(BITCOIN_PRICE_ROUTE, startUnixTimestamp, endUnixTimestamp);
            result = await httpClient.GetAndParseAsync<JArray>(x, null, cancellationToken);
        }
        catch(Exception ex)
        {
            logger.LogError(ex.Message);
            return null;
        }

        return ParsePriceFromPayload(result);
    }

    private static int ParsePriceFromPayload(JArray payload)
    {
        // Access the inner JArray
        var innerArray = payload[0];
        
        // In the API documentation is specified that close value is the value with index 2
        return (int)innerArray[2];
    }
}
