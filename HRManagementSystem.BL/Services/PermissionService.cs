using HRManagementSystem.BL.DTOs.AuthDTO;
using HRManagementSystem.BL.Interfaces;
using HRManagementSystem.DAL.Data.Context;
using HRManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;

public class PermissionService : IPermissionService
{
    private readonly HRContext _context;

    public PermissionService(HRContext context)
    {
        _context = context;
    }

    public async Task AssignPermissionsToRole(string roleId, List<int> permissionIds)
    {
        var existing = _context.RolePermissions.Where(rp => rp.RoleId == roleId);
        _context.RolePermissions.RemoveRange(existing);

        var newPermissions = permissionIds.Select(pid => new RolePermission
        {
            RoleId = roleId,
            PermissionId = pid
        });

        await _context.RolePermissions.AddRangeAsync(newPermissions);
        await _context.SaveChangesAsync();
    }

    public async Task<List<string>> GetUserPermissionsAsync(string userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return new List<string>();

        var roles = await (from userRole in _context.UserRoles
                           join role in _context.Roles on userRole.RoleId equals role.Id
                           where userRole.UserId == userId
                           select role).ToListAsync();

        var roleIds = roles.Select(r => r.Id).ToList();

        var permissions = await (from rp in _context.RolePermissions
                                 join p in _context.Permissions on rp.PermissionId equals p.Id
                                 where roleIds.Contains(rp.RoleId)
                                 select $"{p.Page}-{p.Action}").Distinct().ToListAsync();

        return permissions;
    }
    public async Task<List<PermissionDto>> GetAllPermissionsAsync()
    {
        return await _context.Permissions
            .Select(p => new PermissionDto
            {
                Id = p.Id,
                Page = p.Page,
                Action = p.Action
            }).ToListAsync();
    }

    public async Task<List<int>> GetPermissionsByRoleIdAsync(string roleId)
    {
        return await _context.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .Select(rp => rp.PermissionId)
            .ToListAsync();
    }


    public async Task RemovePermissionsFromRole(string roleId)
    {
        var rolePermissions = await _context.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .ToListAsync();

        _context.RolePermissions.RemoveRange(rolePermissions);
        await _context.SaveChangesAsync();
    }

}
