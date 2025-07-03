using HRManagementSystem.BL.DTOs.AuthDTO;
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
        Task<IdentityResult> CreateUserAsync(UserDto model);

        Task<IdentityResult> AddRoleToUserAsync(string userId, string roleName);
        IEnumerable<RoleDto> GetRolesAsync();
        Task<List<UserViewDto>> GetAllUsersAsync();
        Task<UserViewDto?> GetUserByIdAsync(string id);
        Task<IdentityResult> UpdateUserAsync(string id, UserDto dto);
        Task<IdentityResult> DeleteUserAsync(string id);

    }
}
