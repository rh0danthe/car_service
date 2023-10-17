using AutoMapper;
using car_service.Dto;
using car_service.Entities;

namespace car_service.Helper;

public class MappingProfiles : Profile
{
    public MappingProfiles() 
    {
        CreateMap<CarDto, Car>();
        CreateMap<OrderDto, Order>();
        CreateMap<ClientDto, Client>();
    }
}