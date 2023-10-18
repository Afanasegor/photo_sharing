using Microsoft.EntityFrameworkCore.Design;

namespace PhotoSharing.Data.Context
{
    public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            return new ApplicationContext();
        }
    }
}
