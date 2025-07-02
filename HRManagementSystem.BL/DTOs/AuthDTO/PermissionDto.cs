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
        public string Page { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
    }
}
