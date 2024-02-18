using Application.Rooms.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Rooms.Mappings;
public class DiscountPercentageResolver : IValueResolver<Room, RoomDto, double>
{
    public double Resolve(Room source, RoomDto destination, double destMember, ResolutionContext context)
    {
        return source.Discounts
            .Select(d => d.DiscountPercentage)
            .FirstOrDefault();
    }
}

