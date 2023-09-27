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

            //Binding appsettings.json to AppConfig
            builder.Configuration.SettingsBinding();

            // Add services to the container.
            builder.ConfigureAutofacContainer();

            builder.Services.AddDbContexts();

            builder.Services.AddControllers();
       
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(o => o.AddSwaggerDocumentation());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}