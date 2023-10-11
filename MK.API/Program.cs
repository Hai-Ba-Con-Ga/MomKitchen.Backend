using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using MK.API.Configuration;
using MK.Application.Configuration;
using MK.Domain.Configuration;
using Newtonsoft.Json;
using System.Reflection;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MK.API;

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

        builder.Services.AddFirebase();

        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddCache();

        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

        builder.Services.AddJwtService();

        builder.Services.AddControllers().AddJsonOptions(x =>
        {
            x.JsonSerializerOptions.ReferenceHandler = null;
            x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        // Add services to the container.
        builder.ConfigureAutofacContainer();

        builder.Services.AddDbContexts();

        builder.Services.AddApiVersion();

        builder.Services.AddControllers()
            .AddConfigApiBehaviorOptions();

        builder.Services.AddFluentValidationSetting();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        builder.Services.AddSwaggerGenOption();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseSwagger();

        app.UseSwaggerUI(option => option.EnablePersistAuthorization());

        app.ConfigureExceptionHandler(app.Environment.IsDevelopment());

        builder.Services.SeedData().GetAwaiter().GetResult();

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}