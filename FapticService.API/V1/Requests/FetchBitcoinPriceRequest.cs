using FapticService.API.V1.Responses;
using FluentValidation;
using MediatR;

namespace FapticService.API.V1.Requests;

public class FetchBitcoinPriceRequest : IRequest<FetchBitcoinPriceResponse>
{
    public DateTime? UtcTime { get; set; }
}

public class FetchBitcoinPriceRequestValidator : AbstractValidator<FetchBitcoinPriceRequest>
{
    public FetchBitcoinPriceRequestValidator()
    {
        RuleFor(request => request.UtcTime)
            .GreaterThanOrEqualTo(DateTimeOffset.FromUnixTimeSeconds(1231459200).UtcDateTime)
            .When(request => request.UtcTime.HasValue)
            .WithMessage("Timestamp must a more recent valid UNIX timestamp in seconds");
    }
}
