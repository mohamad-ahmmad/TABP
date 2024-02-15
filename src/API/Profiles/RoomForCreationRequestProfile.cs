using API.Models;
using Application.Rooms.Dtos;
using AutoMapper;
#pragma warning disable CS1591
namespace API.Profiles;
public class RoomForCreationRequestProfile : Profile
{
    public RoomForCreationRequestProfile()
    {
        CreateMap<RoomForCreationRequest, RoomForCreationDto>();
    }
}
