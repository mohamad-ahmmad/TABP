using Application.Abstractions.Messaging;
using Application.Cities.Dtos;
using Application.Dtos;
using Domain.Enums;

namespace Application.Cities.Queries.GetCities;
public record GetCitiesQuery(
    string? SearchTerm,
    int page,
    int pageSize,
    string? sortCol,
    string? sortOrder,
    UserLevels Role) : ICachedQuery<PagedList<CityDto>>
{
    public string CacheKey => $"Cities-{Role}-{SearchTerm}-{page}-{pageSize}-{sortCol}-{sortOrder}";
    public TimeSpan? Expiration => null;
}

