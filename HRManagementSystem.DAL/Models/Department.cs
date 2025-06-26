
using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.DAL.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        // Navigation Properties
        public ICollection<ApplicationUser> Employees { get; set; }
    }
}
