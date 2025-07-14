using HRManagementSystem.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.BL.DTOs.Setting
{
    public class AddSettingDto
    {
        [Required]
        public SettingType Type { get; set; }

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
