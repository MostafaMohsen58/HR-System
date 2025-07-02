using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.DAL.Models
{
    public class Role : IdentityRole
    {
        public ICollection<RolePermission> RolePermissions { get; set; }
    }

}
