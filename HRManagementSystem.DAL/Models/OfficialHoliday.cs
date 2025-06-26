using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.DAL.Models
{
    public class OfficialHoliday
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public DateTime Date { get; set; }
    }
}
