using Application.Amenities.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Amenities.Mappings;
public class AmenityProfile : Profile
{
    public AmenityProfile()
    {
        CreateMap<AmenityForCreationDto, Amenity>();
        CreateMap<Amenity, AmenityDto>();
    }
}
