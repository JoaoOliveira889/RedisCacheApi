namespace RedisCache.Service.Services;

public class CacheRepository<T>(IDistributedCache cache) : ICacheRepository<T>
    where T : class
{
    private readonly string _keyPrefix = $"{typeof(T).Name}:";
    private  readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(10);
    private string GetCacheKey(string id) => $"{_keyPrefix}{id}";
    
    public async Task<T?> GetAsync(string id)
    {
        string key = GetCacheKey(id);
        string? cachedJson = await cache.GetStringAsync(key);

        return cachedJson is null ? null : JsonSerializer.Deserialize<T>(cachedJson);
    }

    public async Task SetAsync(string id, T item, TimeSpan? expiration = null)
    {
        string key = GetCacheKey(id);
        string json = JsonSerializer.Serialize(item);

        var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(expiration ?? _defaultExpiration);
                
        await cache.SetStringAsync(key, json, options);
    }

    public Task RemoveAsync(string id)
    {
        string key = GetCacheKey(id);
        return cache.RemoveAsync(key);
    }
}