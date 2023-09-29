namespace MK.API.Configuration
{
    public static class AppsettingsRegister
    {
        public static void SettingsBinding(this IConfiguration configuration)
        {
            do
            {
                AppConfig.ConnectionStrings = new ConnectionStrings();
                AppConfig.FirebaseConfig = new FirebaseConfig();
                AppConfig.JwtSetting = new JwtSetting();
            }
            while (AppConfig.ConnectionStrings == null);

            configuration.Bind("ConnectionStrings", AppConfig.ConnectionStrings);
            configuration.Bind("FirebaseConfig", AppConfig.FirebaseConfig);
            configuration.Bind("JwtSetting", AppConfig.JwtSetting);
        }

    }
}
