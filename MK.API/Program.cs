using FluentValidation.AspNetCore;
using MK.API.Configuration;
using MK.Domain.Configuration;
using Newtonsoft.Json;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MK.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.ConfigureKestrel((context, serverOptions) =>
            {
                var kestrelSection = context.Configuration.GetSection("Kestrel");

                serverOptions.Configure(kestrelSection);
            });

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

            //Binding appsettings.json to AppConfig
            builder.Configuration.SettingsBinding();

            // Add services to the container.
            builder.ConfigureAutofacContainer();

            builder.Services.AddDbContexts();

            builder.Services.AddApiVersion();

            builder.Services.AddControllers()
                .AddConfigApiBehaviorOptions();

            builder.Services.AddFluentValidationSetting();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(o => o.AddSwaggerDocumentation());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();

            app.UseSwaggerUI();

            app.ConfigureExceptionHandler(app.Environment.IsDevelopment());

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}