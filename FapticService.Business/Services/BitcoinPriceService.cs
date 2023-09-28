
using FapticService.Domain.Models;
using FapticService.Domain.Repository;
using FapticService.Domain.Services;
using Microsoft.Extensions.Logging;

namespace FapticService.Business.Services;

public class BitcoinPriceService : IBitcoinPriceService
{
    private readonly ILogger<BitcoinPriceService> logger;
    private readonly IEnumerable<IBitcoinPriceSource> bitcoinPriceSources;
    private readonly IPricePointRepository pricePointRepository;
    
    public BitcoinPriceService(
        ILogger<BitcoinPriceService> logger,
        IEnumerable<IBitcoinPriceSource> bitcoinPriceSources,
        IPricePointRepository pricePointRepository)
    {
        this.logger = logger;
        this.bitcoinPriceSources = bitcoinPriceSources;
        this.pricePointRepository = pricePointRepository;
    }
    
    /// <inheritdoc />
    public async Task<(double price, int sourcesFetched)> FetchAndAggregate(DateTime timestamp, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching and aggregating prices");
        
        //Check database
        var pricePoint = await pricePointRepository.GetByTimestamp(timestamp, cancellationToken);
        if (pricePoint == null)
        {
            // Fetch sources
            logger.LogInformation("Price not found locally. Fetching from sources");
            var prices = await FetchSources(timestamp, cancellationToken);
            var aggregation = AggregatePrices(prices);

            // Store the result
            pricePoint = new PricePoint
            {
                Price = aggregation,
                SourcesFetched = prices.Count(),
                Timestamp = timestamp
            };
            await pricePointRepository.CreateAsync(pricePoint, cancellationToken);
        }

        return (pricePoint.Price, pricePoint.SourcesFetched);
    }

    private async Task<IEnumerable<int>> FetchSources(DateTime timestamp, CancellationToken cancellationToken)
    {
        var prices = new List<int>();
        foreach (var source in bitcoinPriceSources)
        {
            var price = await source.FetchBitcoinPrice(timestamp, cancellationToken);
            
            // In case a source is unavailable at some moment we just ignore and fetch the others
            if (price.HasValue == false)
            {
                logger.LogWarning($"Failed to fetch from {source.GetType().Name}");
                continue;
            }
            
            prices.Add(price.Value);
        }
        return prices;
    }

    private static double AggregatePrices(IEnumerable<int> prices)
    {
        // Even though this is a single line I still prefer to do it in a separate method in case of implementation changes
        return prices.Average();
    }
}
