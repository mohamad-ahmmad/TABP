using Application.Abstractions.Messaging;
using Application.HotelTypes.Dtos;

namespace Application.HotelTypes.Queries.GetHotelTypeById;
public record GetHotelTypeByIdQuery(Guid HotelTypeId) : ICachedQuery<HotelTypeDto>
{
    public string CacheKey => $"hotel-types-{HotelTypeId}";

    public TimeSpan? Expiration => null;
}

