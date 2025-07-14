using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.BL.DTOs.EmployeeDTO
{
    public class EmployeesByDepartmentDto
    {
        public string DepartmentName { get; set; }
        public int EmployeeCount { get; set; }
    }
}