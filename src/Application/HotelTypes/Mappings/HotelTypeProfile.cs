using Application.HotelTypes.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.HotelTypes.Mappings;
public class HotelTypeProfile : Profile
{
    public HotelTypeProfile()
    {
        CreateMap<HotelType, HotelTypeDto>();
        CreateMap<HotelTypeDto, HotelType>();
    }
}

