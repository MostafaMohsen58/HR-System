using HRManagementSystem.BL.DTOs.AuthDTO;
using HRManagementSystem.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(AuthenticationSchemes = "mySchema", Roles = "Admin")] 
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return user == null ? NotFound(new { message = "User not found" }) : Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.CreateUserAsync(model);
            return !result.Succeeded
                ? BadRequest(new { message = "Failed to create user", errors = result.Errors })
                : Ok(new { message = "User created successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.UpdateUserAsync(id, model);
            return !result.Succeeded
                ? BadRequest(new { message = "Failed to update user", errors = result.Errors })
                : Ok(new { message = "User updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            return !result.Succeeded
                ? BadRequest(new { message = "Failed to delete user", errors = result.Errors })
                : Ok(new { message = "User deleted successfully" });
        }

        [HttpPost("{userId}/roles")]
        public async Task<IActionResult> AddRoleToUser(string userId, [FromBody] string roleName)
        {
            var result = await _userService.AddRoleToUserAsync(userId, roleName);
            return !result.Succeeded
                ? BadRequest(new { message = "Failed to assign role", errors = result.Errors })
                : Ok(new { message = "Role assigned successfully" });
        }
    }
}