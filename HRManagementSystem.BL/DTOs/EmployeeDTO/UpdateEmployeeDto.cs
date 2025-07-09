using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRManagementSystem.DAL.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
//using HRManagementSystem.BL.DTOs.EmployeeDTO.HRManagementSystem.DAL.Models.Enums;

namespace HRManagementSystem.BL.DTOs.EmployeeDTO
{

    public class UpdateEmployeeDto
    {
        [Required]
        public string Id { get; set; }


        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]

        public int? DepartmentId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 1000000)]
        public decimal BaseSalary { get; set; }


        [Required]
        [EmailAddress]

        public string Email { get; set; }

        [MinLength(6)]
        public string? Password { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        [RegularExpression(@"^01[0125][0-9]{8}$")]
        public string PhoneNumber { get; set; }

        [Required]
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        [Required]
        [StringLength(50)]
        public string Nationality { get; set; }

        [Required]
        [StringLength(14, MinimumLength = 14)]
        [RegularExpression(@"^[23][0-9]{13}$")]
        public string NationalId { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.Date)]
        public DateTime ContractDate { get; set; }

        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }






       
     
    }



}
