using Application.Abstractions.Messaging;
using Application.Cities.Dtos;
using Domain.Enums;

namespace Application.Cities.Queries.GetCityById;
public record GetCityByIdCommand(Guid CityId, UserLevels Role) : ICachedQuery<CityDto>
{
    public string CacheKey => $"CityDto-{Role}-{CityId}";
    public TimeSpan? Expiration => null;
}
