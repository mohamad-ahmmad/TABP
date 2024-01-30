using Application.RoomTypes.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.RoomTypes.Mappings;
public class RoomTypeProfile : Profile
{
    public RoomTypeProfile()
    {
        CreateMap<RoomTypeForCreateDto, RoomType>();
        CreateMap<RoomType, RoomTypeDto>();
    }
}

