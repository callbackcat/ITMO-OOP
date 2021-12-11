using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Reports.Auth.Controllers.Resources;
using Reports.Auth.Core.Models;
using Reports.Auth.Core.Services;
using Reports.Auth.Enums;

namespace Reports.Auth.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserCredentialsResource userCredentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<UserCredentialsResource, User>(userCredentials);

            var response = await _userService.CreateUserAsync(user, ApplicationRole.Employee);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            var userResource = _mapper.Map<User, UserResource>(response.User);
            return Ok(userResource);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _userService.FindByIdAsync(id);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.User);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _userService.GetAllUsers();
            if (response is null)
            {
                return BadRequest("Error getting the list of users");
            }

            return Ok(response);
        }

        [HttpPut("{role}")]
        public async Task<IActionResult> AddUserRoleAsync(Guid id, ApplicationRole role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _userService.AddRoleAsync(id, role);

            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> RemoveUserRoleAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _userService.RemoveAsync(id);

            return Ok();
        }
    }
}