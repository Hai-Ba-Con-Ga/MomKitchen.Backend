using Microsoft.Extensions.Caching.Distributed;
using MK.Application.Cache;
using System.Text.Json;

namespace MK.Infrastructure.Cache
{
    public class CacheManager : ICacheManager
    {
        //config json serialize entity
        private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = null,
            AllowTrailingCommas = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        private static readonly DistributedCacheEntryOptions _defaultCacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        };

        private IDistributedCache _distributedCache;


        public CacheManager(IDistributedCache cache)
        {
            _distributedCache = cache;
        }

        public async Task<(bool, T)> GetAsync<T>(string key)
        {
            try
            {
                var rawGetReult = await _distributedCache.GetAsync(key);

                if (rawGetReult == null)
                {
                    var getResult = JsonSerializer.Deserialize<T>(rawGetReult, _serializerOptions);

                    return (true, getResult);
                }
            }
            catch (Exception)
            {
            }

            return (false, default(T));
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _distributedCache.RemoveAsync(key);
            }
            catch (Exception)
            {
            }
        }

        public async Task SetAsync(string key, object value, DistributedCacheEntryOptions options = null)
        {
            if (options == null)
            {
                options = _defaultCacheOptions;
            }

            try
            {
                var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, _serializerOptions));
                await _distributedCache.SetAsync(key, bytes, options);
            }
            catch (Exception)
            {
            }
        }
    }
}
