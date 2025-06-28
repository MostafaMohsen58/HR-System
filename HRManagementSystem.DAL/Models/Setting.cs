

using HRManagementSystem.DAL.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRManagementSystem.DAL.Models
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Type { get; set; } = "default";

        [Required]
        [EnumDataType(typeof(WeekDay))]
        public WeekDay FirstHoliday { get; set; } = WeekDay.Friday;

        [Required]
        [EnumDataType(typeof(WeekDay))]
        public WeekDay SecondHoliday { get; set; } = WeekDay.Saturday;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OverTime { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Deduction { get; set; }
    }
}
