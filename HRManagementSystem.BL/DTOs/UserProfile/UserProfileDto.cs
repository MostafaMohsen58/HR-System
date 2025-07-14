using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.BL.DTOs.UserProfile
{
    public class UserProfileDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Nationality { get; set; }
        public string NationalId { get; set; }
        public decimal Salary { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime ContractDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string DepartmentName { get; set; }

        public string Role { get; set; }
        public List<string> Permissions { get; set; }

        public List<MonthlyAttendanceSummaryDto> MonthlySummaries { get; set; }
    }
    public class MonthlyAttendanceSummaryDto
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int AbsentDays { get; set; }
        public decimal DeductionAmount { get; set; }
    }
}
