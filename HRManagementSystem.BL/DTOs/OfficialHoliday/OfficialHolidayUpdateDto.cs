using System;
using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.BL.DTOs.OfficialHoliday
{
    public class OfficialHolidayUpdateDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}