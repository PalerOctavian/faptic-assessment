namespace FapticService.API.V1.Responses;

public class FetchBitcoinPriceResponse
{
    public double Price { get; set; }
    
    public int SourceFetched { get; set; }
}