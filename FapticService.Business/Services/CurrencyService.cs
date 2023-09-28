using FapticService.Domain.Services;
using Microsoft.Extensions.Logging;

namespace FapticService.Business.Services;

public class CurrencyService : ICurrencyService
{
    private readonly ILogger<CurrencyService> logger;

    public CurrencyService(ILogger<CurrencyService> logger)
    {
        this.logger = logger;
    }
    
    public Task<bool> ConversionSupported(string from, string to)
    {
        logger.LogInformation($"Currency conversion from {from} to {to} supported");
        return Task.FromResult(false);
    }
}