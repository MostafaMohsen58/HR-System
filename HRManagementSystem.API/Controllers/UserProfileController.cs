using System.Security.Claims;
using HRManagementSystem.BL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _profileService;

        public UserProfileController(IUserProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            // جلب الـ userId من الـ Claims
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated");

            var profile = await _profileService.GetUserProfileAsync(userId);
            if (profile == null)
                return NotFound("User not found");

            return Ok(profile);
        }
    }

}
