using HRManagementSystem.BL.DTOs.AuthDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRManagementSystem.BL.Interfaces
{
    public interface IPermissionService
    {
        Task AssignPermissionsToRole(string roleId, List<PermissionDto> permissions);

        Task<List<string>> GetUserPermissionsAsync(string userId);
        Task<List<PermissionDto>> GetAllPermissionsAsync();

        Task<List<int>> GetPermissionsByRoleIdAsync(string roleId);
        Task RemovePermissionsFromRole(string roleId);
        Task<List<PermissionDto>> GetPermissionsDtoByRoleIdAsync(string roleId);

    }
}
