using Application.Abstractions.Messaging;
using Application.RoomTypes.Dtos;

namespace Application.RoomTypes.Queries.GetAllRoomTypes;
public record GetAllRoomTypesQuery : ICachedQuery<IEnumerable<RoomTypeDto>?>
{
    public string CacheKey => "room-types";

    public TimeSpan? Expiration => null;
}

