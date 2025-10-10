namespace RedisCache.Service.Interfaces;

public interface ICacheRepository<T> where T : class
{
    Task<T?> GetAsync(string id); 
    Task SetAsync(string id, T item, TimeSpan? expiration = null); 
    Task RemoveAsync(string id);
}