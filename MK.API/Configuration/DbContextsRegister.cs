

using Autofac;
using MapsterMapper;
using Npgsql;
using System.Data;

namespace MK.API.Configuration
{
    public static class DbContextsRegister
    {
        public static void AddDbContexts(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(AppConfig.ConnectionStrings.DefaultConnection).UseSnakeCaseNamingConvention();
            });
        }
        public static void AddDbContexts(this ContainerBuilder builder)
        {
            //builder.RegisterType<ServiceMapper>().As<IMapper>().InstancePerLifetimeScope();.AddDbContext<ApplicationDbContext>(options =>
            //{
            //    options.UseNpgsql(AppConfig.ConnectionStrings.DefaultConnection).UseSnakeCaseNamingConvention();
            //});

            builder.Register(c => new NpgsqlConnection(AppConfig.ConnectionStrings.DefaultConnection))
                    .As<IDbConnection>()
                    .InstancePerLifetimeScope();
            
            builder.RegisterType<ApplicationDbContext>().As<DbContext>().InstancePerLifetimeScope();
        }
    }
}
