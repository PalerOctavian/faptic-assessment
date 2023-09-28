using FapticService.API.V1.Requests;
using FapticService.API.V1.Responses;
using Refit;

namespace FapticService.IntegrationTesting.RefitInterfaces;

public interface IPriceControllerEndpoints
{
    [Get("/faptic-service/v1/price/btc")]
    Task<FetchBitcoinPriceResponse> GetBitcoinPrice(
        FetchBitcoinPriceRequest request,
        CancellationToken cancellationToken = default);
    
    [Get("/faptic-service/v1/price/btc/range")]
    Task<GetBitcoinPriceTimeRangeResponse> GetBitcoinPriceTimeRange(
        GetBitcoinPriceTimeRangeRequest request,
        CancellationToken cancellationToken = default);
}
