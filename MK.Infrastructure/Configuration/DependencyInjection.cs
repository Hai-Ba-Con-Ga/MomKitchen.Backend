
using Autofac;
using MK.Application.Cache;
using MK.Infrastructure.Cache;
using MK.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MK.Infrastructure.Configuration
{
    public static class DependencyInjection
    {
        public static void RegisterRepository(this ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                 .Where(t => t.Name.EndsWith("Repository"))
                 .AsImplementedInterfaces()
                 .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(GenericRepository<>))
                     .As(typeof(IGenericRepository<>)).InstancePerDependency();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            builder.RegisterType<CacheManager>().As<ICacheManager>().InstancePerLifetimeScope();

        }
    }
}
