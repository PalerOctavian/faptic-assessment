using FapticService.Domain.Repository;
using FapticService.IntegrationTesting.Utils;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Xunit;

namespace FapticService.IntegrationTesting.Fixture;

public class BaseControllerTests<T> : IClassFixture<FapticServiceFixture>
{
    protected readonly T refitInterface;
    private readonly FapticServiceFixture fixture;

    private IPricePointRepository? priceRepository;
    
    public IPricePointRepository PriceRepository => priceRepository ?? (priceRepository = fixture.Server.Services.CreateScope().ServiceProvider.GetRequiredService<IPricePointRepository>());

    public BaseControllerTests(FapticServiceFixture fixture)
    {
        this.fixture = fixture;
        refitInterface = RestService.For<T>(fixture.Client, new RefitSettings(new CustomContentSerializer()));
    }
}
