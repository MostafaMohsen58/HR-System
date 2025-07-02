using HRManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.DAL.Models
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }
        public string Page { get; set; }
        public string Action { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
    }


}

