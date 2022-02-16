using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace gym_app_backend.Extensions
{
    public static class FirebaseExtensions
    {
        public static void AddFirebaseAdmin(this IServiceCollection service, IConfiguration config)
        {
            try
            {
                var privateKeyPath = config["Firebase:PrivateKey"];
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(privateKeyPath)
                });
                Console.WriteLine("Successfully Added Firebase Admin.");
            } 
            catch (Exception ex)
            {
                Console.WriteLine($"AddFirebaseAdmin: {ex.Message}");
                throw;
            }

        }
    }
}
