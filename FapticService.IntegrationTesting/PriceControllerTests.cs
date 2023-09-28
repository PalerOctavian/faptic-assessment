using System.Globalization;
using System.Net;
using FapticService.API.V1.Requests;
using FapticService.Domain.Models;
using FapticService.IntegrationTesting.Fixture;
using FapticService.IntegrationTesting.RefitInterfaces;
using Refit;
using Xunit;

namespace FapticService.IntegrationTesting;

public class PriceControllerTests : BaseControllerTests<IPriceControllerEndpoints>
{
    private const string TEST_DATE = "2023-01-01T00:00:00Z";
    private const double EXPECTED_PRICE = 16546;

    public PriceControllerTests(FapticServiceFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task When_Fetch_Price_Returned_From_Sources()
    {
        // Arrange
        var request = new FetchBitcoinPriceRequest
        {
            UtcTime = DateTime.Parse(TEST_DATE, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal)
        };

        // Act
        var result = await refitInterface.GetBitcoinPrice(request, new CancellationToken());
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.SourceFetched > 0); // Maybe some sources aren't available but it's very unlikely that all source are down
        Assert.Equal(EXPECTED_PRICE, result.Price);
    }
    
    [Fact]
    public async Task When_Fetch_Price_Returned_From_Database()
    {
        // Arrange
        int sources = 999;
        var request = new FetchBitcoinPriceRequest
        {
            UtcTime = DateTime.Parse(TEST_DATE,  CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal).AddDays(1)
        };
        // Setting an absurd number for sources so we know that is retrieved from db
        await PriceRepository.CreateAsync(new PricePoint
            {Price = EXPECTED_PRICE, Timestamp = request.UtcTime.Value, SourcesFetched = sources});

        // Act
        var result = await refitInterface.GetBitcoinPrice(request, new CancellationToken());
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(sources, result.SourceFetched); // Maybe some sources aren't available but it's very unlikely that all source are down
        Assert.Equal(EXPECTED_PRICE, result.Price);
    }
    
    [Fact]
    public async Task When_Fetch_No_Time_Price_Returned()
    {
        // Arrange
        var request = new FetchBitcoinPriceRequest();

        // Act
        var result = await refitInterface.GetBitcoinPrice(request, new CancellationToken());
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.SourceFetched > 0); // Maybe some sources aren't available but it's very unlikely that all source are down
        Assert.True(result.Price > 0);
    }

    [Fact]
    public async Task When_Fetch_Range_No_Content_Returned()
    {
        // Arrange
        var time = DateTime.Parse(TEST_DATE, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
        var request = new GetBitcoinPriceTimeRangeRequest
        {
            Start = time.AddDays(10),
            End = time.AddDays(11)
        };
        
        // Act
        var result = await refitInterface.GetBitcoinPriceTimeRange(request, new CancellationToken());
        
        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task When_Fetch_Range_Correct_Range_Returned()
    {
        // Arrange
        var time = DateTime.Parse(TEST_DATE, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
        
        await PriceRepository.CreateAsync(new PricePoint {Timestamp = time, Price = 100, SourcesFetched = 4});
        await PriceRepository.CreateAsync(new PricePoint {Timestamp = time.AddDays(2), Price = 100, SourcesFetched = 4});
        await PriceRepository.CreateAsync(new PricePoint {Timestamp = time.AddDays(4), Price = 100, SourcesFetched = 4});
        
        var request = new GetBitcoinPriceTimeRangeRequest
        {
            Start = time.AddDays(1),
            End = time.AddDays(4)
        };
        
        // Act
        var result = await refitInterface.GetBitcoinPriceTimeRange(request, new CancellationToken());
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Prices.Count());
    }
    
    [Fact]
    public async Task When_Fetch_Start_Greater_Than_End()
    {
        // Arrange
        var time = DateTime.Parse(TEST_DATE, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
        var request = new GetBitcoinPriceTimeRangeRequest
        {
            Start = time.AddDays(1),
            End = time
        };
        
        // Act
        try
        {
            var result = await refitInterface.GetBitcoinPriceTimeRange(request, new CancellationToken());
        }
        catch (ValidationApiException ex)
        {
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);
        }
        catch (Exception ex)
        {
            Assert.Fail();
        }
    }
}
