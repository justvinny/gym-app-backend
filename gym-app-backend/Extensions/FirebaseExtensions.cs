using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using gym_app_backend.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
                Log.Logger.Information("Successfully Added Firebase Admin.");
            }
            catch (Exception ex)
            {
                Log.Logger.Error($"AddFirebaseAdmin: {ex.Message}");
                throw;
            }
        }

        // Referenced from this blog https://blog.markvincze.com/secure-an-asp-net-core-api-with-firebase/
        public static void AddFirebaseAuthentication(this IServiceCollection service, IConfiguration configuration)
        {
            var firebaseOptions = new FirebaseOptions();
            configuration.GetSection(FirebaseOptions.Firebase).Bind(firebaseOptions);

            var projectUrl = $"{firebaseOptions.ServiceUrl}{firebaseOptions.ProjectId}";

            service
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = projectUrl;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = projectUrl,
                        ValidateAudience = true,
                        ValidAudience = firebaseOptions.ProjectId,
                        ValidateLifetime = true,
                    };
                });

            Log.Logger.Information($"AddFirebaseAuthentication successfully added with project url: {projectUrl}");
        }
    }
}
