using HRManagementSystem.BL.DTOs.AuthDTO;
using HRManagementSystem.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "mySchema", Roles = "Admin")]
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
            if (user == null)
                return NotFound("User not found.");
            return Ok(user);
        }
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserDto model)
        {
            var result = await _userService.CreateUserAsync(model);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var user = await _userService.GetUserByUsernameAsync(model.UserName); 

            return Ok(new
            {
                message = "User created successfully.",
                userId = user.Id  
            });
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserDto model)
        {
            var result = await _userService.UpdateUserAsync(id, model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "User updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "User deleted successfully." });
        }

        [HttpPost("{userId}/roles")]
        public async Task<IActionResult> AddRoleToUser(string userId, [FromBody] string roleName)
        {
            var result = await _userService.AddRoleToUserAsync(userId, roleName);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Role assigned to user successfully." });
        }
    }
}
