using Application.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Caching;

public class InMemoryCacheService : ICacheService
{
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(5);
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<InMemoryCacheService> _logger;

    public InMemoryCacheService(IMemoryCache memoryCache, ILogger<InMemoryCacheService> logger)
    {
        _memoryCache = memoryCache;
        _logger = logger;
    }
    public async Task<T> GetOrCreateAsync<T>(string cacheKey, Func<CancellationToken, Task<T>> factory, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        if(_memoryCache.TryGetValue(cacheKey, out T? cachedData)) 
        {
            _logger.LogInformation($"Cache hit.");
            return cachedData!;        
        }
        else
        {
            _logger.LogInformation($"Cache miss.");
            T data = await factory(cancellationToken);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                   .SetAbsoluteExpiration(expiration ?? DefaultExpiration);

           _memoryCache.Set(cacheKey, data);

            return data;
        }
    }
}

