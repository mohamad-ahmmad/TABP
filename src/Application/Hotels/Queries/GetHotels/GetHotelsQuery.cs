using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Dtos;
using Application.Hotels.Dtos;
using Domain.Enums;

namespace Application.Hotels.Queries.GetHotels;
public record GetHotelsQuery(string? SortCol,
    string? SortOrder,
    string? SearchTerm,
    int Page,
    int PageSize,
    UserLevels UserLevel) : ICachedQuery<PagedList<HotelDto>>
{
    public string CacheKey => $"hotels-{SortOrder}-{SearchTerm}-{Page}-{PageSize}-{UserLevel}";

    public TimeSpan? Expiration => null;
}
