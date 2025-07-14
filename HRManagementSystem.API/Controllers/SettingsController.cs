using HRManagementSystem.BL.DTOs.Setting;
using HRManagementSystem.BL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingService _settingService;
        public SettingsController(ISettingService settingService)
        {
            _settingService = settingService;
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddSettingDto settingDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdId = await _settingService.AddSettingAsync(settingDTO);
            if (createdId == 0)
            {
                return BadRequest(new { message = "Failed to create setting." });
            }
            return CreatedAtAction(nameof(Get), new { id = createdId }, settingDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var setting = await _settingService.GetSettingAsync();
                return Ok(setting);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] EditSettingDto setting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _settingService.UpdateSettingAsync(setting);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
