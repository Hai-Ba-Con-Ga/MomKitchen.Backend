using Microsoft.Extensions.Caching.Distributed;
using MK.Application.Cache;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MK.Infrastructure.Cache
{
    public class CacheManager : ICacheManager
    {
        private readonly IDatabase _redisCache;

        private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = null,
            AllowTrailingCommas = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public CacheManager()
        {
            _redisCache = InitConnection();
        }

        private IDatabase InitConnection()
        {
            try
            {
                var configuration = ConfigurationOptions.Parse(AppConfig.CacheConfig.RedisConnectionString);
                var redisConnection = ConnectionMultiplexer.Connect(configuration);
                return redisConnection.GetDatabase();
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetBaseException();
                throw new Exception("Can not create redis connection: " + errorMessage);
            }
        }

        public async Task<(bool, T?)> GetAsync<T>(string key)
        {
            try
            {
                string rawGetReult = await _redisCache.StringGetAsync(key);

                if (rawGetReult == null)
                    return (true, default(T));

                var getResult = JsonSerializer.Deserialize<T>(rawGetReult, _serializerOptions);

                return (true, getResult);
            }
            catch (Exception)
            {
                return (false, default(T));
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _redisCache.KeyDeleteAsync(key);
            }
            catch (Exception)
            {
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpTime = null, TimeSpan? unusedExpTime = null)
        {
            try
            {
                var rawData = JsonSerializer.Serialize(value, _serializerOptions);
                await _redisCache.StringSetAsync(key, rawData, AppConfig.CacheConfig.AbsoluteExpTime);
            }
            catch (Exception)
            {
            }
        }


        #region Disable
        //config json serialize entity


        //private IDistributedCache _distributedCache;


        //public CacheManager(IDistributedCache cache)
        //{
        //    _distributedCache = cache;
        //}

        //public async Task<(bool, T?)> GetAsync<T>(string key)
        //{
        //    try
        //    {
        //        var rawGetReult = await _distributedCache.GetAsync(key);

        //        if (rawGetReult != null)
        //        {
        //            var getResult = JsonSerializer.Deserialize<T>(rawGetReult, _serializerOptions);

        //            return (true, getResult);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }

        //    return (false, default(T));
        //}

        //public async Task RemoveAsync(string key)
        //{
        //    try
        //    {
        //        await _distributedCache.RemoveAsync(key);
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        //public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpTime = null, TimeSpan? unusedExpTime = null)
        //{
        //    var option = new DistributedCacheEntryOptions
        //    {
        //        AbsoluteExpirationRelativeToNow = absoluteExpTime ?? AppConfig.CacheConfig.AbsoluteExpTime,
        //        SlidingExpiration = unusedExpTime ?? AppConfig.CacheConfig.UnusedExpTime
        //    };

        //    try
        //    {
        //        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, _serializerOptions));
        //        await _distributedCache.SetAsync(key, bytes, option);
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        #endregion Disable
    }
}
