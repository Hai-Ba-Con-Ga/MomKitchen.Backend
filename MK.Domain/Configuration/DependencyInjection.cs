
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFluentValidationSetting(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                .AddFluentValidationAutoValidation(config =>
            {
                config.ImplicitlyValidateChildProperties = false;
                config.ImplicitlyValidateRootCollectionElements = true;
            });

            return services;
        }
    }
}
