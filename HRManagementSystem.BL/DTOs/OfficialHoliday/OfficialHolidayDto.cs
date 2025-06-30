using System;
using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.BL.DTOs.OfficialHoliday
{
    public class OfficialHolidayDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}