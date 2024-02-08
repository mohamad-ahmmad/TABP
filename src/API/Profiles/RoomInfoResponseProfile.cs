using API.Models;
using Application.RoomInfos.Dtos;
using AutoMapper;

namespace API.Profiles;
public class RoomInfoResponseProfile : Profile
{
    public RoomInfoResponseProfile()
    {
        CreateMap<RoomInfoDto, RoomInfoResponse>();
    }
}

