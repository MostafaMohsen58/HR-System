using HRManagementSystem.BL.DTOs.PayRoll;
using HRManagementSystem.BL.Interfaces;
using HRManagementSystem.BL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayRollController : ControllerBase
    {
        private readonly IPayRollService _payRollService;
        public PayRollController(IPayRollService payRollService)
        {
            _payRollService = payRollService;
        }
        [HttpPost]
        public async Task<IActionResult> AddAndUpdatePayRollAsync(PayRollRequestDto request)
        {
            try
            {
                var result = await _payRollService.AddPayRollAsync(request.Month, request.Year, request.EmployeeId, request.CheckIn, request.CheckOut);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllPayRollsAsync()
        {
            try
            {
                var result = await _payRollService.GetAllPayRollsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeletePayRollAsync(PayRollRequestDto request)
        {
            try
            {
                var result = await _payRollService.DeletePayRollAsync(request.Month, request.Year, request.EmployeeId, request.CheckIn, request.CheckOut);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginated(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 5,
            string? searchTerm = null,
            int? month = null,
            int? year = null)
        {
            try
            {
                var result = await _payRollService.GetPaginatedAttendancesAsync(pageIndex, pageSize, searchTerm, month, year);
                if (result.Items.Count == 0)
                {
                    string message;

                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        message = $"No employee found with name '{searchTerm.Trim()}' Please enter valid employee name";
                    }
                    else
                    {
                        message = $"No records found for date range";
                    }

                    return NotFound(new { error = message });
                }

                return Ok(result);

            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }
    }
}
