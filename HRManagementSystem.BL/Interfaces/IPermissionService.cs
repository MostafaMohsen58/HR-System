using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRManagementSystem.BL.DTOs.AuthDTO;


namespace HRManagementSystem.BL.Interfaces
{
  public interface IPermissionService
    {
        Task AssignPermissionsToRole(string roleId, List<int> permissionIds);
        Task<List<string>> GetUserPermissionsAsync(string userId);
        Task<List<int>> GetPermissionsByRoleIdAsync(string roleId);
        Task<List<PermissionDto>> GetAllPermissionsAsync();
        Task RemovePermissionsFromRole(string roleId);


    }
}
