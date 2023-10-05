

using Autofac;
using MapsterMapper;
using MK.Domain.Entity;
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
        //seed data
        public static async Task SeedData(this IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.MigrateAsync();
            if (!context.Roles.Any())
            {
                List<Role> roles = new List<Role>(
                    new Role[]
                    {
                        new Role
                        {
                            Name = "Admin",

                        },
                        new Role
                        {
                            Name = "Customer",
                        },
                        new Role
                        {
                            Name = "Kitchen",
                        },
                    }

                    );
                await context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync();

            }
        }

    }
}
