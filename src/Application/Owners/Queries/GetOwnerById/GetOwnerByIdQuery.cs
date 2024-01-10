using Application.Abstractions.Messaging;
using Application.Owners.DTOs;

namespace Application.Owners.Queries.GetOwnerById;
public record GetOwnerByIdQuery(Guid OwnerId) : ICachedQuery<OwnerDto?>
{
    public string CacheKey => $"owner-{OwnerId}";

    public TimeSpan? Expiration => null;
}

