using Amazon.Auth.AccessControlPolicy;
using FluentValidation;
using MK.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Service.Configuration
{
    public static class ConfigTypeAdapter
    {
        public static void ConfigTypeAdapters()
        {
            TypeAdapterConfig.GlobalSettings.NewConfig<UpdateDishReq, Dish>().IgnoreNullValues(true);
        }

    }
}
