using Application.Users.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Users.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserForCreateDTO, User>()
            .ForMember(u => u.UserLevel, (config) =>config.Ignore());

        CreateMap<User, UserDto>();
    }
}

