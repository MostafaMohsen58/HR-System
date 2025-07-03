using HRManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.BL.DTOs.AttendanceDTOs
{
    public class AttendanceUpdateDto
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime ArrivalTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime DepartureTime { get; set; }

        [Required]
        public string EmployeeId { get; set; }
    }
}
