using Microsoft.EntityFrameworkCore;
using Reports.Auth.Core.Models;

namespace Reports.Auth.Data
{
    public class AuthDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(builder);

            builder.Entity<UserRole>().HasKey(ur => new
            {
                ur.UserId,
                ur.RoleId
            });
        }
    }
}