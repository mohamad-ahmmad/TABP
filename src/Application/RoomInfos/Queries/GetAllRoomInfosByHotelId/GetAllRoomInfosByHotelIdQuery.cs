using Application.Abstractions.Messaging;
using Application.RoomInfos.Dtos;

namespace Application.RoomInfos.Queries.GetAllRoomInfosByHotelId;
public record GetAllRoomInfosByHotelIdQuery(Guid HotelId) : ICachedQuery<IEnumerable<RoomInfoDto>>
{
    public string CacheKey => $"room-infos-{HotelId}";

    public TimeSpan? Expiration => null;
}

