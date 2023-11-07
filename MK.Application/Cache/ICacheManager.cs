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
        //Task<(bool, T?)> GetAsync<T>(string key);
        //Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpTime = null, TimeSpan? unusedExpTime = null);
        //Task RemoveAsync(string key);
    }
}
