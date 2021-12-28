using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reports.Auth.Core.Models;
using Reports.Auth.Core.Repositories;
using Reports.Auth.Enums;

namespace Reports.Auth.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _context;

        public UserRepository(AuthDbContext context) =>
            _context = context;

        public async Task AddAsync(User user, IEnumerable<ApplicationRole> userRoles)
        {
            var roleNames = userRoles.Select(r => r.ToString()).ToList();
            var roles = await _context.Roles
                .Where(r => roleNames.Contains(r.Name))
                .ToListAsync();

            foreach(var role in roles)
            {
                user.UserRoles.Add(new UserRole
                {
                    RoleId = role.Id
                });
            }

            _context.Users.Add(user);
        }

        public async Task RemoveAsync(Guid userId)
        {
            var user = FindByIdAsync(userId).Result;
            _context.Users.Remove(user);
        }

        public async Task AddRoleAsync(Guid userId, ApplicationRole role)
        {
            var newRole = await _context.Roles
                .SingleAsync(r => r.Name == role.ToString());

           var user = FindByIdAsync(userId).Result;

           user.UserRoles.Add(new UserRole
           {
               RoleId = newRole.Id,
               UserId = user.Id,
               Role = newRole
           });
        }

        public async Task<User> FindByEmailAsync(string email) =>
            await _context.Users.Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(u => u.Email == email);

        public async Task<User> FindByIdAsync(Guid id) =>
            await _context.Users.Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(u => u.Id == id);

        public async Task<List<User>> GetAllUsers() =>
            await _context.Users.ToListAsync();
    }
}