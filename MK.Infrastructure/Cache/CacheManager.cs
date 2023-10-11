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

        public Task ClearAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync<T>(string key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsSetAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task SetAsync(string key, object value, TimeSpan? timeSpan = null, DistributedCacheEntryOptions options = null)
        {
            if (options == null)
            {
                options = new DistributedCacheEntryOptions();
            }

            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, _serializerOptions));
            return _distributedCache.SetAsync(key, bytes, options);
        }



        public Task SetAsync<TSource, TResult>(string key, QueryHelper<TSource, TResult> queryHelper, TimeSpan? timeSpan = null)
            where TSource : BaseEntity
            where TResult : class
        {
            throw new NotImplementedException();
        }
    }
}
