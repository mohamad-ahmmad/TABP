using Application.Abstractions.Messaging;
using Application.Hotels.Dtos;

namespace Application.Hotels.Queries.GetHotelById;

public record GetHotelByIdQuery(Guid HotelId) : ICachedQuery<HotelDto>
{
    public string CacheKey => $"hotels-{HotelId}";

    public TimeSpan? Expiration => null;
}
