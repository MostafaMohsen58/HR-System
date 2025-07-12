using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HRManagementSystem.DAL.Models
{
    public class OfficialHoliday
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
    }
}
