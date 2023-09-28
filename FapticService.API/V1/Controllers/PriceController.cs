using FapticService.API.V1.Requests;
using FapticService.API.V1.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FapticService.API.V1.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("faptic-service/v{version:apiVersion}/[controller]")]
public class PriceController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly ILogger<PriceController> logger;

    public PriceController(
        IMediator mediator,
        ILogger<PriceController> logger)
    {
        this.mediator = mediator;
        this.logger = logger;
    }

    [HttpGet("btc")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<FetchBitcoinPriceResponse>> GetBitcoinPrice(
        [FromQuery] FetchBitcoinPriceRequest request,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Get bitcoin price request");
        if (request.UtcTime.HasValue == false)
        {
            request.UtcTime = DateTime.UtcNow;
        }
        
        var result = await mediator.Send(request, cancellationToken);

        if (result.SourceFetched == 0)
        {
            return NoContent();
        }
        return Ok(result);
    }
    
    [HttpGet("btc/range")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<GetBitcoinPriceTimeRangeResponse>> GetBitcoinPriceTimeRange(
        [FromQuery] GetBitcoinPriceTimeRangeRequest request,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Get bitcoin price request in time range");
        var result = await mediator.Send(request, cancellationToken);

        if (!result.Prices.Any())
        {
            return NoContent();
        }
        return Ok(result);
    }
}