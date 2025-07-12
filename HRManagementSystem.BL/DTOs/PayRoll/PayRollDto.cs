using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.BL.DTOs.PayRoll
{
    public class PayRollDto
    {
        public string? EmployeeName { get; set; }
        public string? DepartmentName { get; set; }
        public decimal BasicSalary { get; set; }
        public int PresentDays { get; set; }

        public int AbsentDays { get; set; }
        public int ExtraHours { get; set; }

        public int DeductionInHours { get; set; }
        public decimal TotalAddition { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal NetSalary { get; set; }
    }
}
