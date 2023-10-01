namespace MK.API.Configuration
{
    public static class ConfigApiVersioning
    {
        public static void AddApiVersion(this IServiceCollection services)
        {
            // Configure API versioning
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0); // Set the default API version
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            // Other service configurations...
        }
    }
}
