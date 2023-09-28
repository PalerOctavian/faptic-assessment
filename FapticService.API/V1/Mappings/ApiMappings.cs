using AutoMapper;
using FapticService.API.V1.Dto;
using FapticService.Domain.Models;

namespace FapticService.API.V1.Mappings;

public class ApiMappings : Profile
{
    public ApiMappings()
    {
        CreateMap<PricePoint, PricePointDto>();
    }
}