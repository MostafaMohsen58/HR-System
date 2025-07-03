using HRManagementSystem.BL.DTOs.AttendanceDTOs;
using HRManagementSystem.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _attendanceService.GetAllAttendancesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error retrieving attendances: {ex.Message}" });
            }
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginated([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 5)
        {
            try
            {
                var result = await _attendanceService.GetPaginatedAttendancesAsync(pageIndex, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error retrieving paginated attendances: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var record = await _attendanceService.GetAttendanceByIdAsync(id);
                return Ok(record);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {error = $"Error retrieving attendance by ID: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AttendanceDto attendanceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var newId = await _attendanceService.AddAttendanceAsync(attendanceDto);
                return CreatedAtAction(nameof(GetById), new { id = newId }, new { id = newId, date = attendanceDto.Date, arrivalTime = attendanceDto.ArrivalTime, departureTime = attendanceDto.DepartureTime, employeeId = attendanceDto.EmployeeId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {error = $"Error adding attendance: {ex.Message}" });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AttendanceUpdateDto attendanceUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedRecord = await _attendanceService.UpdateAttendanceAsync(attendanceUpdateDto);
                return Ok(updatedRecord);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {error = $"Error updating attendance: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _attendanceService.DeleteAttendanceAsync(id);
                return Ok(new { message = $"Attendance with id: {id} is deleted successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {error = $"Error deleting attendance: {ex.Message}" });
            }
        }
    }
}
