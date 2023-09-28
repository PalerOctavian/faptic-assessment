using AutoMapper;
using FapticService.API.V1.Mappings;

namespace FapticService.Configuration;

public static class AutoMapperConfiguration
{
    public static void AddAutoMapping(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton(_ => new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ApiMappings());
        }).CreateMapper());
    }
}