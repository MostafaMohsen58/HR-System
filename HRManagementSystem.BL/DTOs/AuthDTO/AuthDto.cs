using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.BL.DTOs.AuthDTO
{
    public class AuthDto
    {
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public string FullName { get; set; }
    }
}
