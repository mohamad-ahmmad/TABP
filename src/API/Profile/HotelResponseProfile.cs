using API.Models;
using Application.Hotels.Dtos;
namespace API.Profile;
#pragma warning disable CS1591

public class HotelResponseProfile : AutoMapper.Profile
{
    public HotelResponseProfile()
    {
        CreateMap<HotelDto, HotelResponse>();
    }
}