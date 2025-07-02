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
        public bool IsView { get; set; }
        public bool IsAdd { get; set; }
        public bool IsDelete { get; set; }
        public bool IsEdit { get; set; }
        public string Page { get; set; }


        public ICollection<RolePermission> RolePermissions { get; set; }
    }


}

