

using System.Reflection;

namespace MK.API.Configuration
{
    public static class ConfigureSwaggerOptions
    {
        public static void AddSwaggerDocumentation(this SwaggerGenOptions options)
        {

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        }

        public static IServiceCollection AddSwaggerGenOption(this IServiceCollection service)
        {
            return service.AddSwaggerGen(c =>
               {
                   c.SwaggerDoc("v1", new OpenApiInfo
                   {
                       Title = "MK API",
                       Version = "v1",
                       Description = "API for Mom Kitchen Project",
                       Contact = new OpenApiContact
                       {
                           Name = "Contact Developers",
                           Url = new Uri("https://github.com/Hai-Ba-Con-Ga")
                       }
                   });
                   c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                   {
                       Description = "Standard Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                       In = ParameterLocation.Header,
                       Name = "Authorization",
                       Type = SecuritySchemeType.ApiKey
                   });
                   c.AddSwaggerDocumentation();
               });
        }
    }
}
