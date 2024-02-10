using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Dtos;
using Application.Hotels.Dtos;
using Domain.Enums;

namespace Application.Hotels.Queries.GetHotels;
public record GetHotelsQuery(int? MinPrice,
    int? MaxPrice,
    double? HotelRating,
    string? Amenities,
    string? HotelType,
    string? RoomType,
    string? SortCol,
    string? SortOrder,
    string? SearchTerm,
    int? NumberOfAdults,
    int? NumberOfChildren,
    int? NumberOfRooms,
    int Page,
    int PageSize,
    UserLevels UserLevel) : ICachedQuery<PagedList<HotelDto>>
{
    public string CacheKey => $"hotels-{SortOrder}-{SearchTerm}-{Page}" +
        $"-{MinPrice}-{MaxPrice}-{HotelRating}" +
        $"-{Amenities}-{RoomType}-{SortCol}" +
        $"-{HotelType}-{PageSize}-{UserLevel}" +
        $"-{NumberOfAdults}-{NumberOfChildren}-{NumberOfRooms}";

    public TimeSpan? Expiration => null;
}
