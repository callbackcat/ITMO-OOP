using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reports.Auth.Core.Models;
using Reports.Auth.Enums;

namespace Reports.Auth.Core.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user, IEnumerable<ApplicationRole> userRoles);
        Task AddRoleAsync(Guid userId, ApplicationRole role);
        Task RemoveAsync(Guid userId);
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByIdAsync(Guid id);
        Task<List<User>> GetAllUsers();
    }
}