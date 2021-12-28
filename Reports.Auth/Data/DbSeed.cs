using System.Collections.Generic;
using System.Linq;
using Reports.Auth.Core.Models;
using Reports.Auth.Enums;

namespace Reports.Auth.Data
{
    public class DbSeed
    {
        public static void Seed(AuthDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Roles.Any()) return;

            var roles = new List<Role>
            {
                new Role { Name = ApplicationRole.Employee.ToString() },
                new Role { Name = ApplicationRole.TeamLead.ToString() }
            };

            context.Roles.AddRange(roles);
            context.SaveChanges();
        }
    }
}