namespace MK.API.Configuration
{
    public static class AppsettingsRegister
    {
        public static void SettingsBinding(this IConfiguration configuration)
        {
            do
            {
                AppConfig.ConnectionStrings = new ConnectionStrings();
            }
            while (AppConfig.ConnectionStrings == null);

            configuration.Bind("ConnectionStrings", AppConfig.ConnectionStrings);
            configuration.Bind("FirebaseConfigPath", AppConfig.FirebaseConfigPath);
            configuration.Bind("JwtSetting", AppConfig.JwtSetting);
        }

    }
}
