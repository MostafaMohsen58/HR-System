using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HRManagementSystem.DAL.Models.Enums;

namespace HRManagementSystem.BL.DTOs.SEttingDTO
{
    public class EditSettingDTO
    {
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
