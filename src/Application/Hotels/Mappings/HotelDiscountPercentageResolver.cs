using Application.Hotels.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Hotels.Mappings;
public class HotelDiscountPercentageResolver : IValueResolver<Hotel, HotelDto, double?>
{
    public double? Resolve(Hotel source, HotelDto destination, double? destMember, ResolutionContext context)
    {
        return source?.
            RoomInfos.FirstOrDefault()?.
            Rooms.FirstOrDefault()?.
            Discounts.FirstOrDefault()?.
            DiscountPercentage;
    }
}
