using Application.Owners.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Owners.Mappings;
public class OwnerProfile : Profile
{
    public OwnerProfile()
    {
        CreateMap<OwnerForCreateDto, Owner>();
        CreateMap<Owner, OwnerDto>(); 
        CreateMap<OwnerDto, Owner>();
    }
}
