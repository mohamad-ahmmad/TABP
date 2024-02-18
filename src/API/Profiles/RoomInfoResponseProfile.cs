using API.Models;
using Application.RoomInfos.Dtos;
using AutoMapper;
#pragma warning disable CS1591
namespace API.Profiles;
public class RoomInfoResponseProfile : Profile
{
    public RoomInfoResponseProfile()
    {
        CreateMap<RoomInfoDto, RoomInfoResponse>();
    }
}

