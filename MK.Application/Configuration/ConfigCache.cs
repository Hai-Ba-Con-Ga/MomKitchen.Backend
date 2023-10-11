using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Application.Configuration
{
    public static class ConfigCache
    {
        public static void AddCache(this IServiceCollection service)
        {
            service.AddStackExchangeRedisCache(options =>
            {
                options.InstanceName = "MK.Application";
                options.Configuration = AppConfig.ConnectionStrings.RedisConnection;
            });
        }
    }
}
