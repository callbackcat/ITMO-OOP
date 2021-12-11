using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reports.Auth.Core.Models;
using Reports.Auth.Core.Services.Communication;
using Reports.Auth.Enums;

namespace Reports.Auth.Core.Services
{
    public interface IUserService
    {
         Task<CreateUserResponse> CreateUserAsync(User user, params ApplicationRole[] userRoles);
         Task RemoveAsync(Guid userId);
         Task AddRoleAsync(Guid userId, ApplicationRole role);
         Task<User> FindByEmailAsync(string email);
         Task<CreateUserResponse> FindByIdAsync(Guid id);
         Task<List<User>> GetAllUsers();
    }
}