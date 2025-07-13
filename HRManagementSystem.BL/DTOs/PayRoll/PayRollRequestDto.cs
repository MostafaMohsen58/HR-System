using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.BL.DTOs.PayRoll
{
    public class PayRollRequestDto
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string EmployeeId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
    }
}
