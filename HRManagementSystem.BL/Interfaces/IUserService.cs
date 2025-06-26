using HRManagementSystem.BL.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.BL.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterEmployeeDto model, string role);
        Task<AuthDto> LoginUserAsync(LoginDto model);
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<IdentityResult> AddRoleToUserAsync(string userId, string roleName);
        Task<IEnumerable<RoleDto>> GetRolesAsync();
    }
}
