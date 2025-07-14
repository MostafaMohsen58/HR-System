using Microsoft.AspNetCore.Identity;

namespace HRManagementSystem.DAL.Models
{
    public class Role : IdentityRole
    {
        public ICollection<RolePermission> RolePermissions { get; set; }
    }

}
