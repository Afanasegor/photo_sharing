using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PhotoSharing.Core.Models.Auth;
using PhotoSharing.Core.Models.Common;
using PhotoSharing.Core.Models.Social;
using FileInfo = PhotoSharing.Core.Models.Common.FileInfo;

namespace PhotoSharing.Data.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        public ApplicationContext() : base()
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<FileInfo> FileInfos { get; set; }


        public static bool IsMigration = true;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured && IsMigration)
            {
                IConfiguration configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

                var connStr = configuration.GetConnectionString("PostgreConnection");

                if (string.IsNullOrWhiteSpace(connStr))
                    connStr = "";

                optionsBuilder.UseNpgsql(connStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var hasher = new PasswordHasher<User>();

            var adminGuid = Guid.Parse("ddd42cbe-45e3-42f4-975e-cb393c6849d2");
            var userGuid = Guid.Parse("31e53e1e-a297-4b88-8f06-bcd8aa120a43");

            string adminRoleName = "admin";
            string userRoleName = "user";

            string adminEmail = "admin@mail.ru";
            string adminPassword = "123456";

            string userEmail = "user@mail.ru";
            string userPassword = "123456";


            var adminRole = new Role { Id = adminGuid, Name = adminRoleName };
            var userRole = new Role { Id = userGuid, Name = userRoleName };

            var adminUser = new User { Id = adminGuid, Email = adminEmail, RoleId = adminRole.Id };
            var userUser = new User { Id = userGuid, Email = userEmail, RoleId = userRole.Id };

            adminUser.PasswordHash = hasher.HashPassword(adminUser, adminPassword);
            userUser.PasswordHash = hasher.HashPassword(userUser, userPassword);

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser, userUser });



            // models schema
            modelBuilder.Entity<Friendship>()
              .HasKey(fs => new { fs.UserFirstId, fs.UserSecondId});

            modelBuilder.Entity<Friendship>()
                .HasOne(fs => fs.UserFirst)
                .WithMany(u => u.Subscriptions)
                .HasForeignKey(fs => fs.UserFirstId);

            modelBuilder.Entity<Friendship>()
                .HasOne(fs => fs.UserSecond)
                .WithMany(u => u.Subscribers)
                .HasForeignKey(fs => fs.UserSecondId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Files)
                .WithOne(f => f.User)
                .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);

            base.OnModelCreating(modelBuilder);
        }
    }
}
