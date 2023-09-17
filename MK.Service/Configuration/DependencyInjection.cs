namespace MK.Service.Configuration
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("Service") || t.Name.EndsWith("ApiHelper"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }

        public static void RegisterMapster(this ContainerBuilder builder)
        {
            //scan all assemblies finding for IRegister
            var config = TypeAdapterConfig.GlobalSettings;
            var assemblies = new Assembly[] 
            { 
                Assembly.GetExecutingAssembly(), 
                typeof(BaseEntity).Assembly 
            };
            config.Scan(assemblies);

            builder.RegisterInstance(config).SingleInstance();
            builder.RegisterType<ServiceMapper>().As<IMapper>().InstancePerLifetimeScope();
        }

    }
}
