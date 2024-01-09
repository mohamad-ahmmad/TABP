using Application.Abstractions.Messaging;
using Application.Dtos;
using Application.Owners.DTOs;

namespace Application.Owners.Queries;
public record GetOwnersQuery(string? SearchTerm, string? PhoneNumber, int Page = 1, int PageSize = 30) : ICachedQuery<PagedList<OwnerDto>>
{
    public string CacheKey => $"owners-{Page}-{PageSize}-{SearchTerm}-{PhoneNumber}";

    public TimeSpan? Expiration => null;
}

