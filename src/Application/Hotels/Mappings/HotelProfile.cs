using Application.Hotels.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Hotels.Mappings;
public class HotelProfile : Profile
{
    public HotelProfile()
    {
        CreateMap<HotelForCreateDto, Hotel>();
        CreateMap<Hotel, HotelDto>();
    }
}

