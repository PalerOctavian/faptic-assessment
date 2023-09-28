using FapticService.API.V1.Responses;
using FluentValidation;
using MediatR;

namespace FapticService.API.V1.Requests;

public class GetBitcoinPriceTimeRangeRequest : IRequest<GetBitcoinPriceTimeRangeResponse>
{
    public DateTime Start { get; set; }

    public DateTime End { get; set; }
}

public class GetBitcoinPriceTimeRangeRequestValidator : AbstractValidator<GetBitcoinPriceTimeRangeRequest>
{
    public GetBitcoinPriceTimeRangeRequestValidator()
    {
        RuleFor(x => x.End)
            .GreaterThan(x => x.Start)
            .WithMessage("EndDate must be greater than StartDate.");
    }
}