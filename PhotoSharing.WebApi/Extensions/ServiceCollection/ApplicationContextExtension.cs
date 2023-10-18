using Microsoft.EntityFrameworkCore;
using PhotoSharing.Data.Context;

namespace PhotoSharing.WebApi.Extensions.ServiceCollection
{
    public static class ApplicationContextExtension
    {
        public static void AddApplicationContextPostgreSQL(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionsString = configuration.GetConnectionString("PostgreConnection");

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddDbContext<ApplicationContext>(options => options
                .UseNpgsql(connectionsString));
        }

        public static void ApplyDatabaseMigrations(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                var pendingMigrations = context.Database.GetPendingMigrations();
                if (pendingMigrations.Count() > 0)
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
