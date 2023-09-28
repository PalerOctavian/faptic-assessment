using FapticService.API.V1.Requests;
using FapticService.API.V1.Responses;
using FapticService.Common.Extensions;
using FapticService.Domain.Exceptions;
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
    private readonly ICurrencyService currencyService;

    public FetchBitcoinPriceHandler(
        ILogger<FetchBitcoinPriceHandler> logger,
        IBitcoinPriceService bitcoinPriceService,
        ICurrencyService currencyService)
    {
        this.logger = logger;
        this.bitcoinPriceService = bitcoinPriceService;
        this.currencyService = currencyService;
    }
    
    public override async Task<FetchBitcoinPriceResponse> Handle(FetchBitcoinPriceRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handling Bitcoin price fetching for timestamp {request.UtcTime}");
        var timestampWithPrecision = request.UtcTime!.Value.ToHourPrecision();

        var result = await bitcoinPriceService.FetchAndAggregate(timestampWithPrecision, cancellationToken);

        return new FetchBitcoinPriceResponse
        {
            SourceFetched = result.sourcesFetched,
            Price = result.price
        };
    }
}
