using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reports.Auth.Core.Models;
using Reports.Auth.Core.Repositories;
using Reports.Auth.Core.Security.Hashing;
using Reports.Auth.Core.Services;
using Reports.Auth.Core.Services.Communication;
using Reports.Auth.Enums;

namespace Reports.Auth.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<CreateUserResponse> CreateUserAsync(User user, params ApplicationRole[] userRoles)
        {
            var existingUser = await _userRepository.FindByEmailAsync(user.Email);
            if (existingUser != null)
            {
                return new CreateUserResponse(false, "Email already in use", null);
            }

            user.Password = _passwordHasher.HashPassword(user.Password);
            user.Id = Guid.NewGuid();

            await _userRepository.AddAsync(user, userRoles);
            await _unitOfWork.CompleteAsync();

            return new CreateUserResponse(true, null, user);
        }

        public async Task RemoveAsync(Guid userId) =>
            await _userRepository.RemoveAsync(userId);

        public async Task AddRoleAsync(Guid userId, ApplicationRole role) =>
            await _userRepository.AddRoleAsync(userId, role);

        public async Task<User> FindByEmailAsync(string email) =>
            await _userRepository.FindByEmailAsync(email);

        public async Task<CreateUserResponse> FindByIdAsync(Guid id)
        {
            var user = await _userRepository.FindByIdAsync(id);
            return user is null
                ? new CreateUserResponse(false, "User wasn't found", null)
                : new CreateUserResponse(true, null, user);
        }

        public Task<List<User>> GetAllUsers() =>
            _userRepository.GetAllUsers();
    }
}