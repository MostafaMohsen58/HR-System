
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRManagementSystem.DAL.Models
{
    public class PayRoll
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, 12)]
        public int Month { get; set; }

        [Required]
        [Range(2000, 2100)]
        public int Year { get; set; }

        [Required]
        [Range(0, 31)]
        public int DaysPresent { get; set; }

        [Required]
        [Range(0, 31)]
        public int AbsentDays { get; set; }

        [Required]
        [Range(0, 31)]
        public int DeductedDays { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int ExtraHours { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BasicSalary { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetSalary { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAddition { get;set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDeduction { get;set; }

        //Foreign Key
        [Required]
        public string EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public ApplicationUser Employee { get; set; }


        public void CalculateNetSalary()
        {
            NetSalary = BasicSalary + TotalAddition - TotalDeduction;
        }
    }
}
