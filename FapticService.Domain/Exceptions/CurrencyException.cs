namespace FapticService.Domain.Exceptions;

public class CurrencyException : ApplicationException
{
    public CurrencyException(string message) : base(message) { }
    
    public CurrencyException(string message, Exception? innerException) : base(message, innerException) { }
}