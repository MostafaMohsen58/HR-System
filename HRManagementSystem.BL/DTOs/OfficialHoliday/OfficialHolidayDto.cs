using System;
using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.BL.DTOs
{
    public class OfficialHolidayDto
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
    }
}