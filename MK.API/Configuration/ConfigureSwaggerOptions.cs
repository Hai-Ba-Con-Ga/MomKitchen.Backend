

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
    }
}
