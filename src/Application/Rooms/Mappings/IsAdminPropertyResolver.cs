using Application.Abstractions;
using Application.Rooms.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Rooms.Mappings;

public class IsAdminPropertyResolver : IValueResolver<Room, RoomDto, bool>
{
    private readonly IUserContext _userContext;

    public IsAdminPropertyResolver(IUserContext userContext)
    {
        _userContext = userContext; 
    }
    public bool Resolve(Room source, RoomDto destination, bool destMember, ResolutionContext context)
    {
        return _userContext.GetUserLevel() == UserLevels.Admin;
    }
}
