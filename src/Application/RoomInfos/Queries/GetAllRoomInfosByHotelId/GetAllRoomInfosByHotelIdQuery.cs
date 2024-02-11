using Application.Abstractions.Messaging;
using Application.RoomInfos.Dtos;

namespace Application.RoomInfos.Queries.GetAllRoomInfosByHotelId;
public record GetAllRoomInfosByHotelIdQuery(Guid HotelId,
    string? RoomType,
    int? MinPrice,
    int? MaxPrice) : ICachedQuery<IEnumerable<RoomInfoDto>>
{
    public string CacheKey => $"room-infos-{HotelId}-{RoomType}-{MaxPrice}-{MinPrice}";

    public TimeSpan? Expiration => null;
}

