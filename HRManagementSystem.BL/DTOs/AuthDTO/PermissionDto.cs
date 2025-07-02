using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.BL.DTOs.AuthDTO
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string Page { get; set; }
        public bool IsView { get; set; }
        public bool IsAdd { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
    }
}
