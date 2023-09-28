using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace MK.API.Configuration
{
    public static class ConfigureFirebase
    {
        public static void AddFirebase(this IServiceCollection services)
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile("../momkitchen-41f2b-firebase-adminsdk.json")
            });
        }
    }
}
