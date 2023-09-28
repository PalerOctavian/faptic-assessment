using FapticService.Common.Extensions;
using FapticService.Domain.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace FapticService.Business.ApiClients;

public class BitstampApiClient : IBitcoinPriceSource
{
    private const string BITCOIN_PRICE_ROUTE = "api/v2/ohlc/btcusd/?step=3600&limit=1&start={0}";
    
    private readonly ILogger<BitstampApiClient> logger;
    private readonly HttpClient httpClient;

    public BitstampApiClient(
        ILogger<BitstampApiClient> logger,
        IHttpClientFactory httpClientFactory)
    {
        this.logger = logger;
        httpClient = httpClientFactory.CreateClient("bitstamp");
    }
    
    public async Task<int?> FetchBitcoinPrice(DateTime timestamp, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching bitstamp for bitcoin price");

        var unixTimestamp = timestamp.ToUnixTimeSeconds();

        JObject result;
        try
        {
            var x = string.Format(BITCOIN_PRICE_ROUTE, unixTimestamp);
            result = await httpClient.GetAndParseAsync<JObject>(x, null, cancellationToken);
        }
        catch(Exception ex)
        {
            logger.LogError(ex.Message);
            return null;
        }
        
        return ParsePriceFromPayload(result);
    }

    private static int ParsePriceFromPayload(JObject payload)
    {
        string closeValue = (string)payload.SelectToken("data.ohlc[0].close");

        return int.Parse(closeValue);
    }
}
