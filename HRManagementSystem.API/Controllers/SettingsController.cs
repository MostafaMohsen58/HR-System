using HRManagementSystem.BL.DTOs.DepartmentDTO;
using HRManagementSystem.BL.DTOs.SEttingDTO;
using HRManagementSystem.BL.Interfaces;
using HRManagementSystem.BL.Services;
using HRManagementSystem.DAL.Models;
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
        [HttpGet("get")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddSettingDTO settingDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdId = await _settingService.AddSettingAsync(settingDTO);
            return CreatedAtAction(nameof(GetById), new { id = createdId }, settingDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var setting = await _settingService.GetSettingByIdAsync(id);
                return Ok(setting);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EditSettingDTO setting)
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
