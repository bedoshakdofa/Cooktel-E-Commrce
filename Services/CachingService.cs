using Cooktel_E_commrece.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Cooktel_E_commrece.Services
{
    public class CachingService : ICachingService
    {
        private readonly IDistributedCache _cache;

        public CachingService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public T GetData<T>(string key)
        {
            var data=_cache.GetString(key);

            if (data == null)
                return default(T);
            else
                return JsonSerializer.Deserialize<T>(data);
        }

        public void SetData<T>(string key, T data)
        {
            var option = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
            };

            _cache.SetString(key, JsonSerializer.Serialize(data), option);
        }

        public async Task RemoveCache<T>(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
