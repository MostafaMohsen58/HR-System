using HRManagementSystem.BL.DTOs;
using HRManagementSystem.BL.Interfaces;
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
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("HR Management System API is running.");
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.RegisterUserAsync(model);
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
                if (result.Succeeded)
                {
                    return Ok("User logged in successfully.");
                }
                return Unauthorized("Invalid login attempt.");
            }
            return BadRequest(ModelState);
        }
    }
}
