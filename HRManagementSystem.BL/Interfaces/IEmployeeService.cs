using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRManagementSystem.BL.DTOs.DepartmentDTO;
using HRManagementSystem.BL.DTOs.EmployeeDTO;

namespace HRManagementSystem.BL.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<ViewEmployeeDto>> GetAllEmployeesAsync();
        Task<ViewEmployeeDto> GetEmployeeByIdAsync(string id);
        Task<bool> UpdateEmployeeAsync(UpdateEmployeeDto dto);
        Task<bool> DeleteEmployeeAsync(string id);

        Task<IEnumerable<ViewEmployeeDto>> GetEmployeesByDepartmentIdAsync(int departmentId);

    }
}

