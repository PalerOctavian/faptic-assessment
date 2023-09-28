namespace FapticService.Domain.Models;

public class PricePoint
{
    public int Id { get; set; }
    
    public double Price { get; set; }
    
    public int SourcesFetched { get; set; }
    
    public DateTime Timestamp { get; set; }
}