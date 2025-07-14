using HRManagementSystem.BL.DTOs.EmployeeDTO;
using HRManagementSystem.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound(new { message = "Employee not found." });

            return Ok(employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateEmployeeDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "Id in URL does not match Id in body." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _employeeService.UpdateEmployeeAsync(dto);
            if (!result)
                return NotFound(new { message = "Employee not found." });

            return Ok(new { message = "Employee updated successfully." });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _employeeService.DeleteEmployeeAsync(id);
            if (!result)
                return NotFound(new { message = "Employee not found." });

            return Ok(new { message = "Employee deleted successfully." });
        }

        [HttpGet("department/{departmentId}")]
        public async Task<IActionResult> GetEmployeesByDepartment(int departmentId)
        {
            var employees = await _employeeService.GetEmployeesByDepartmentIdAsync(departmentId);
            return Ok(employees);
        }


        [HttpGet("dashboard/totalEmployees")]
        public async Task<IActionResult> GetTotalCount()
        {
            var totalCount = await _employeeService.GetTotalEmployeesCountAsync();
            return Ok(new { totalEmployees = totalCount });
        }

        [HttpGet("dashboard/employeesByDepartment")]
        public async Task<IActionResult> GetEmployeesByDepartment()
        {
            var result = await _employeeService.GetEmployeesByDepartmentAsync();
            return Ok(result);
        }

        [HttpGet("dashboard/genderDistribution")]
        public async Task<IActionResult> GetGenderDistribution()
        {
            var result = await _employeeService.GetGenderDistributionAsync();
            return Ok(result);
        }

        [HttpGet("dashboard/averageSalary")]
        public async Task<IActionResult> GetAverageSalary()
        {
            var avg = await _employeeService.GetAverageSalaryAsync();
            return Ok(new { averageSalary = avg });
        }

        [HttpGet("dashboard/groupByAge")]
        public async Task<IActionResult> GetEmployeesGroupedByAge()
        {
            var result = await _employeeService.GetEmployeesGroupedByAgeAsync();
            return Ok(result);
        }

        [HttpGet("dashboard/nationalityDistribution")]
        public async Task<IActionResult> GetNationalityDistribution()
        {
            var result = await _employeeService.GetNationalityDistributionAsync();
            return Ok(result);
        }
     





    }
}
