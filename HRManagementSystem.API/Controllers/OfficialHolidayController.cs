﻿using HRManagementSystem.BL.DTOs.OfficialHoliday;
using HRManagementSystem.BL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficialHolidayController : ControllerBase
    {
        private readonly IOfficialHolidayService _officialHolidayService;
        
        public OfficialHolidayController(IOfficialHolidayService officialHolidayService)
        {
            _officialHolidayService = officialHolidayService;
        }
        
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] OfficialHolidayDto officialHoliday)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdId = await _officialHolidayService.AddOfficialHolidayAsync(officialHoliday);
            return CreatedAtAction(nameof(GetById), new { id = createdId }, officialHoliday);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var officialHolidays = await _officialHolidayService.GetAllOfficialHolidaysAsync();
            return Ok(officialHolidays);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var officialHoliday = await _officialHolidayService.GetOfficialHolidayByIdAsync(id);
                return Ok(officialHoliday);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] OfficialHolidayUpdateDto officialHoliday)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _officialHolidayService.UpdateOfficialHolidayAsync(officialHoliday);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deletedId = await _officialHolidayService.DeleteOfficialHolidayAsync(id);
                return Ok(new {message = $"Holiday with id: {deletedId} is deleted successfully"});
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
