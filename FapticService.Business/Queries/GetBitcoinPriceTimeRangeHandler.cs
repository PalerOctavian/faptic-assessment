using AutoMapper;
using FapticService.API.V1.Dto;
using FapticService.API.V1.Requests;
using FapticService.API.V1.Responses;
using FapticService.Domain.Repository;
using Microsoft.Extensions.Logging;

namespace FapticService.Business.Queries;

public class GetBitcoinPriceTimeRangeHandler : BaseHandler<GetBitcoinPriceTimeRangeRequest, GetBitcoinPriceTimeRangeResponse>
{
    private readonly ILogger<GetBitcoinPriceTimeRangeHandler> logger;
    private readonly IPricePointReadOnlyRepository pricePointRepository;
    private readonly IMapper mapper;

    public GetBitcoinPriceTimeRangeHandler(
        ILogger<GetBitcoinPriceTimeRangeHandler> logger,
        IPricePointRepository pricePointRepository,
        IMapper mapper)
    {
        this.logger = logger;
        this.pricePointRepository = pricePointRepository;
        this.mapper = mapper;
    }
    
    public override async Task<GetBitcoinPriceTimeRangeResponse> Handle(GetBitcoinPriceTimeRangeRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handling retrieving bitcoin prices in time range {request.Start} - {request.End}");

        var result = await pricePointRepository.GetByTimestamps(
            request.Start,
            request.End,
            cancellationToken);

        return new GetBitcoinPriceTimeRangeResponse
        {
            Prices = mapper.Map<IEnumerable<PricePointDto>>(result)
        };
    }
}
