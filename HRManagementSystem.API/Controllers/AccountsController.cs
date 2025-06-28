using HRManagementSystem.BL.DTOs.AuthDTO;
using HRManagementSystem.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUserService userService;

        public AccountsController(IUserService userService)
        {
            this.userService = userService;
        }
        [Authorize(AuthenticationSchemes = "mySchema")]
        [HttpGet("get")]
        public IActionResult Get()
        {
            return Ok("HR Management System API is running well.");
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterEmployee([FromBody] RegisterEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.RegisterUserAsync(model, "User");
                if (result.Succeeded)
                {
                    return Ok("User registered successfully.");
                }
                return BadRequest(result.Errors);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.LoginUserAsync(model);

                if (result.IsAuthenticated)
                {
                    return Ok(result);
                }
                return BadRequest(result.Message);
            }
            return BadRequest(ModelState);
        }
    }
}
