namespace Application.Abstractions.Messaging;
public interface ICachedQuery<TResponse> : IQuery<TResponse>, ICachedQuey
{
}

public interface ICachedQuey
{
    string CacheKey { get; }
    TimeSpan? Expiration { get; }
}