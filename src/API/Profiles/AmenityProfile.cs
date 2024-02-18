using API.Models;
using Application.Amenities.Dtos;
using AutoMapper;
#pragma warning disable CS1591
namespace API.Profiles;
public class AmenityProfile : Profile
{
    public AmenityProfile()
    {
        CreateMap<AmenityForCreationRequest, AmenityForCreationDto>();
    }
}
