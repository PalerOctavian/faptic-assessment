using FapticService.API.V1.Requests;
using FapticService.API.V1.Responses;
using FapticService.Business.Contract;
using FapticService.Domain.Services;
using Microsoft.Extensions.Logging;

namespace FapticService.Business.Queries;

/// <summary>
///     Fetch bitcoin price with hour precision
/// </summary>
public class FetchBitcoinPriceHandler : BaseHandler<FetchBitcoinPriceRequest, FetchBitcoinPriceResponse>
{
    private readonly ILogger<FetchBitcoinPriceHandler> logger;
    private readonly IBitcoinPriceService bitcoinPriceService;
    private readonly ITimeUtility timeUtility;

    public FetchBitcoinPriceHandler(
        ILogger<FetchBitcoinPriceHandler> logger,
        IBitcoinPriceService bitcoinPriceService,
        ITimeUtility timeUtility)
    {
        this.logger = logger;
        this.bitcoinPriceService = bitcoinPriceService;
        this.timeUtility = timeUtility;
    }
    
    public override async Task<FetchBitcoinPriceResponse> Handle(FetchBitcoinPriceRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handling Bitcoin price fetching for timestamp {request.UtcTime}");
        var timestampWithPrecision = timeUtility.ToHourPrecision(request.UtcTime!.Value);

        var result = await bitcoinPriceService.FetchAndAggregate(timestampWithPrecision, cancellationToken);

        return new FetchBitcoinPriceResponse
        {
            SourceFetched = result.sourcesFetched,
            Price = result.price
        };
    }
}
