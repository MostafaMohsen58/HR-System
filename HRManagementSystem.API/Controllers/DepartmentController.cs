using HRManagementSystem.BL.DTOs.DepartmentDTO;
using HRManagementSystem.BL.Interfaces;
using HRManagementSystem.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace HRManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] DepartmentDto department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdId = await _departmentService.AddDepartmentAsync(department);
            return CreatedAtAction(nameof(GetById), new { id = createdId }, department);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return Ok(departments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var department = await _departmentService.GetDepartmentByIdAsync(id);
                return Ok(department);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] DepartmentUpdateDto department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _departmentService.UpdateDepartmentAsync(department);
                return Ok(result); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deletedId = await _departmentService.DeleteDepartmentAsync(id);
                return Ok($"Department with id: {deletedId} is deleted successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}