using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.BL.DTOs.DepartmentDTO
{
    public class DepartmentWithEmployeeCountDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EmployeeCount { get; set; }
    }
}
