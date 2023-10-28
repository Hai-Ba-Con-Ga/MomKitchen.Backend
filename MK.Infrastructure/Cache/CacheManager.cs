using Microsoft.Extensions.Caching.Distributed;
using MK.Application.Cache;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpTime = null, TimeSpan? unusedExpTime = null)
        {
            var option = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpTime ?? AppConfig.CacheConfig.AbsoluteExpTime,
                SlidingExpiration = unusedExpTime ?? AppConfig.CacheConfig.UnusedExpTime
            };

            try
            {
                var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, _serializerOptions));
                await _distributedCache.SetAsync(key, bytes, option);
            }
            catch (Exception)
            {
            }
        }
    }
}
