﻿using API.Models;
using Application.Amenities.Dtos;
using AutoMapper;

namespace API.Profiles;
public class AmenityProfile : Profile
{
    public AmenityProfile()
    {
        CreateMap<AmenityForCreationRequest, AmenityForCreationDto>();
    }
}
