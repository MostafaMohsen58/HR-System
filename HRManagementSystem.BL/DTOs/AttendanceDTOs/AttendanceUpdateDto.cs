using System.ComponentModel.DataAnnotations;

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

        public string? EmployeeName { get; set; }
        public string? DepartmentName { get; set; }
        public int? departmentId { get; set; }
    }
}
