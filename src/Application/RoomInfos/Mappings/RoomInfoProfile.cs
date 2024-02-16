using Application.RoomInfos.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.RoomInfos.Mappings;
public class RoomInfoProfile : Profile
{
    public RoomInfoProfile()
    {
        CreateMap<RoomInfoForCreateDto, RoomInfo>();
        CreateMap<RoomInfo, RoomInfoDto>();
    }
}

