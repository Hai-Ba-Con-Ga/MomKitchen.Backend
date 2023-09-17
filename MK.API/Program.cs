
using Autofac.Core;

namespace MK.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Binding appsettings.json to AppConfig
            builder.Configuration.SettingsBinding();

            // Add services to the container.
            builder.ConfigureAutofacContainer();

            builder.Services.AddDbContexts();

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //builder.Services.AddVersionedApiExplorer(setup =>
            //{
            //    setup.GroupNameFormat = "'v'VVV";
            //    setup.SubstituteApiVersionInUrl = true;
            //});
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}