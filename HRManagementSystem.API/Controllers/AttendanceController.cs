using HRManagementSystem.BL.DTOs.AttendanceDTOs;
using HRManagementSystem.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

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
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginated(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 5,
            string? searchTerm = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            try
            {
                var result = await _attendanceService.GetPaginatedAttendancesAsync(pageIndex, pageSize, searchTerm, startDate, endDate);
                if (result.Items.Count == 0)
                {
                    string message;

                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        message = $"No employee found with name '{searchTerm.Trim()}'";

                        if (startDate.HasValue || endDate.HasValue)
                        {
                            message += $" for the date range: {FormatDateRange(startDate, endDate)}";
                        }
                    }
                    else
                    {
                        message = $"No records found for date range: {FormatDateRange(startDate, endDate)}";
                    }

                    return NotFound(new { error = message });
                }

                return Ok(result);

            }
            catch(ArgumentException ex)
            {
                return BadRequest(new {error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
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
                return StatusCode(500, new {error = $"Internal server error: {ex.Message}" });
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
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {error = $"Internal server error: {ex.Message}" });
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
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {error = $"Internal server error: {ex.Message}" });
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
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        private string FormatDateRange(DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue)
            {
                return $"{startDate.Value:yyyy-MM-dd} to {endDate.Value:yyyy-MM-dd}";
            }
            if (startDate.HasValue)
            {
                return $"after {startDate.Value:yyyy-MM-dd}";
            }
            if (endDate.HasValue)
            {
                return $"before {endDate.Value:yyyy-MM-dd}";
            }
            return "all dates";
        }
    }
}
