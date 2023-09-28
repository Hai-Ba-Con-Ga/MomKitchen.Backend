using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace MK.API.Configuration
{
    public static class ConfigureAuth
    {
        public static IServiceCollection AddJwtService(this IServiceCollection services)
        {


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>

            {
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = AppConfig.JwtSetting.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(AppConfig.JwtSetting.IssuerSigningKey)),
                    ValidateIssuer = AppConfig.JwtSetting.ValidateIssuer,
                    ValidateAudience = AppConfig.JwtSetting.ValidateAudience,
                };
            }
            );

            return services;
        }
    }
}
