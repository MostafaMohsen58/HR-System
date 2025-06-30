using HRManagementSystem.BL.DTOs.DepartmentDTO;
using HRManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.BL.Interfaces
{
    public interface IDepartmentService
    {
        Task<int> AddDepartmentAsync(DepartmentDto department);
        Task<int> DeleteDepartmentAsync(int id);
        Task<IEnumerable<DepartmentWithEmployeeCountDto>> GetAllDepartmentsAsync();
        Task<DepartmentUpdateDto> GetDepartmentByIdAsync(int id);
        Task<DepartmentUpdateDto> UpdateDepartmentAsync(DepartmentUpdateDto department);
    }
}
