using API.Models;
using Application.Rooms.Dtos;
using AutoMapper;

namespace API.Profiles;
public class RoomForCreationRequestProfile : Profile
{
    public RoomForCreationRequestProfile()
    {
        CreateMap<RoomForCreationRequest, RoomForCreationDto>();
    }
}
