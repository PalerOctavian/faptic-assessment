namespace FapticService.Domain.Services;

public interface ICurrencyService
{
    Task<bool> ConversionSupported(string from, string to);
}