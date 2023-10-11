using Microsoft.Extensions.Caching.Distributed;
using MK.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Cache
{
    public interface ICacheManager
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync(string key, object value, TimeSpan? timeSpan = null, DistributedCacheEntryOptions options = null);
        Task RemoveAsync(string key);
        Task<bool> IsSetAsync(string key);
        Task ClearAsync();
    }
}
