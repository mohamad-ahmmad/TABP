using Application.Abstractions.Messaging;
using Application.Dtos;
using Application.Owners.DTOs;

namespace Application.Owners.Queries.GetOwners;
public record GetOwnersQuery(string? SearchTerm, string? PhoneNumber, int Page, int PageSize) : ICachedQuery<PagedList<OwnerDto>>
{
    public string CacheKey => $"owners-{Page}-{PageSize}-{SearchTerm}-{PhoneNumber}";

    public TimeSpan? Expiration => null;
}

