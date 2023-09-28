using FapticService.API.V1.Dto;

namespace FapticService.API.V1.Responses;

public class GetBitcoinPriceTimeRangeResponse
{
    public IEnumerable<PricePointDto> Prices { get; set; }
}