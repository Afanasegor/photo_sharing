using PhotoSharing.Configuration;

namespace PhotoSharing.WebApi.Extensions.AppInit
{
    public static class ConfigInitialization
    {
        /// <summary>
        /// Priority for config is for env variables and if some fields are empty, it takes from appsettings file (appsettings.secret.json)
        /// </summary>
        public static void InitConfig(IConfiguration configuration, IServiceCollection services)
        {
            var fileDirectory = configuration["FileDirectory"];

            if (string.IsNullOrWhiteSpace(fileDirectory))
            {
                throw new DirectoryNotFoundException("Directory was not set");
            }

            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }

            var config = new AppConfig(fileDirectory);

            // Initialization DI
            services.AddSingleton(config);
        }
    }
}
