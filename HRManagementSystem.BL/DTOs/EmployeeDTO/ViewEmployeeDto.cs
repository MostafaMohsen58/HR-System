using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.BL.DTOs.EmployeeDTO
{
   
        public class ViewEmployeeDto
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Department { get; set; }
            public decimal BaseSalary { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string PhoneNumber { get; set; }
            public string Gender { get; set; }
            public string Nationality { get; set; }
            public string NationalId { get; set; }
            public DateTime BirthDate { get; set; }
            public DateTime ContractDate { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
        }
    }


