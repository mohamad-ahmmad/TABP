using Application.Rooms.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Rooms.Mappings;
public class RoomDtoProfile : Profile
{
    public RoomDtoProfile()
    {
        CreateMap<RoomForCreationDto, Room>();
        CreateMap<Room, RoomDto>()
            .ForMember(rd => rd.IsAdmin,
            opt => opt.MapFrom<IsAdminPropertyResolver>());
    }
}
