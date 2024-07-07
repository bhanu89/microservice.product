using microservice.product.Data.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace microservice.product.Data.Cache
{
    public class MemCacheRepository<T> where T : Entity
    {
        private readonly IMemoryCache _memCache;

        public MemCacheRepository(IMemoryCache memCache){
            _memCache = memCache;
        }

        public T Get(string key)
        {
            _memCache.TryGetValue(key, out T value);
            return value;
        }

        public void Add(T value, TimeSpan? ttl = null)
        {
            if (ttl == null)
            {
                ttl = TimeSpan.FromHours(1);
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = ttl
            };

            _memCache.Set(value.GetCacheKey(), value, cacheEntryOptions);
        }

        public void Delete(string key) { 
            _memCache.Remove(key);
        }
    }
}
