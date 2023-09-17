using Microsoft.AspNetCore.Authorization;

namespace MK.API.Configuration
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            IEnumerable<string> source = (from attr in context.MethodInfo.GetCustomAttributes(inherit: true).OfType<AuthorizeAttribute>()
                                          select attr.Policy).Distinct();
            if (source.Any())
            {
                operation.Responses.TryAdd("401", new OpenApiResponse
                {
                    Description = "Unauthorized"
                });
                operation.Responses.TryAdd("403", new OpenApiResponse
                {
                    Description = "Forbidden"
                });
                OpenApiSecurityScheme openApiSecurityScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "oauth2"
                    }
                };
                operation.Security.Add(new OpenApiSecurityRequirement { [openApiSecurityScheme] = source.ToList() });
            }
        }
    }
}
