using HRManagementSystem.BL.DTOs.AuthDTO;
using HRManagementSystem.BL.Interfaces;
using HRManagementSystem.DAL.Data.Context;
using HRManagementSystem.DAL.Interfaces;
using HRManagementSystem.DAL.Models;
using HRManagementSystem.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace HRManagementSystem.BL.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly HRContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public PermissionService(HRContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task AssignPermissionsToRole(string roleId, List<PermissionDto> permissionDtos)
        {
            var oldRolePermissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Include(rp => rp.Permission)
                .ToListAsync();

            _context.Permissions.RemoveRange(oldRolePermissions.Select(rp => rp.Permission));
            _context.RolePermissions.RemoveRange(oldRolePermissions);
            await _context.SaveChangesAsync();

            var newRolePermissions = new List<RolePermission>();

            foreach (var dto in permissionDtos)
            {
                var permission = new Permission
                {
                    Page = dto.Page,
                    IsView = dto.IsView,
                    IsAdd = dto.IsAdd,
                    IsEdit = dto.IsEdit,
                    IsDelete = dto.IsDelete
                };

                _context.Permissions.Add(permission);
                await _context.SaveChangesAsync();

                newRolePermissions.Add(new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = permission.Id
                });
            }

            _context.RolePermissions.AddRange(newRolePermissions);
            await _context.SaveChangesAsync();
        }

        public async Task<List<string>> GetUserPermissionsAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return new List<string>();

            var roleIds = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToListAsync();

            var permissions = await (from rp in _context.RolePermissions
                                     join p in _context.Permissions on rp.PermissionId equals p.Id
                                     where roleIds.Contains(rp.RoleId)
                                     select p).ToListAsync();

            var result = new List<string>();
            foreach (var p in permissions)
            {
                if (p.IsView) result.Add($"{p.Page}-View");
                if (p.IsAdd) result.Add($"{p.Page}-Add");
                if (p.IsEdit) result.Add($"{p.Page}-Edit");
                if (p.IsDelete) result.Add($"{p.Page}-Delete");
            }

            return result.Distinct().ToList();
        }

        public async Task<List<PermissionDto>> GetAllPermissionsAsync()
        {
            var permissions = await _unitOfWork.PermissionRepository.GetAllAsync();
            return permissions.Select(p => new PermissionDto
            {
                Id = p.Id,
                Page = p.Page,
                IsView = p.IsView,
                IsAdd = p.IsAdd,
                IsEdit = p.IsEdit,
                IsDelete = p.IsDelete
            }).ToList();
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

        public async Task<List<PermissionDto>> GetPermissionsDtoByRoleIdAsync(string roleId)
        {
            var permissionEntities = await _context.RolePermissions
                .Include(rp => rp.Permission)
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.Permission)
                .ToListAsync();

            return permissionEntities.Select(p => new PermissionDto
            {
                Id = p.Id,
                Page = p.Page,
                IsView = p.IsView,
                IsAdd = p.IsAdd,
                IsEdit = p.IsEdit,
                IsDelete = p.IsDelete
            }).ToList();
        }
    }
}