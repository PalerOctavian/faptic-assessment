namespace FapticService.Domain.Exceptions;

public class PriceException : ApplicationException
{
    public PriceException(string message) : base(message) { }
    
    public PriceException(string message, Exception? innerException) : base(message, innerException) { }
}