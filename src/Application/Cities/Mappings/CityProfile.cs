using Application.Cities.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Cities.Mappings;

public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<City, CityForAdminDto>();
    }
}

