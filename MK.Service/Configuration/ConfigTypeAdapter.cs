using Amazon.Auth.AccessControlPolicy;
using FluentValidation;
using MK.Domain.Dto;
using MK.Domain.Dto.Response.Tray;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Service.Configuration
{
    public static class ConfigTypeAdapter
    {
        public static TypeAdapterConfig ConfigCustomMapper(this TypeAdapterConfig config)
        {

           config.NewConfig<Tray, TrayDetailRes>()
                .Map(dest => dest.KitchenName, src => src.Kitchen.Name);




            return config;
        }

    }
}
