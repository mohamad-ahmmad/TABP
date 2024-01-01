namespace Application.Abstractions;
public interface ICacheService
{
    Task<T> GetOrCreateAsync<T>(string cacheKey,
        Func<CancellationToken ,Task<T>> factory,
        TimeSpan? expiration= null,
        CancellationToken cancellationToken= default);
}
