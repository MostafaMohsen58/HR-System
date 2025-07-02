using HRManagementSystem.BL.DTOs.AuthDTO;
using HRManagementSystem.BL.Interfaces;
using HRManagementSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly RoleManager<Role> _roleManager;
    private readonly IPermissionService _permissionService;

    public RoleController(RoleManager<Role> roleManager, IPermissionService permissionService)
    {
        _roleManager = roleManager;
        _permissionService = permissionService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoleWithPermissions([FromBody] RoleDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.RoleName))
            return BadRequest("Group name is required.");

        if (dto.Permissions == null || !dto.Permissions.Any())
            return BadRequest("At least permissions must be specified.");

        var role = new Role { Name = dto.RoleName };
        var result = await _roleManager.CreateAsync(role);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await _permissionService.AssignPermissionsToRole(role.Id, dto.Permissions);

        return Ok(new { message = "The group has been added successfully." });
    }
    [HttpGet]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = _roleManager.Roles
            .Select(r => new RoleViewDto
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();

        return Ok(roles);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoleById(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
            return NotFound("Role not found.");

        var permissionIds = await _permissionService.GetPermissionsByRoleIdAsync(role.Id);

        return Ok(new
        {
            id = role.Id,
            name = role.Name,
            permissions = permissionIds
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole(string id, [FromBody] RoleDto dto)

    {
        var role = await _roleManager.FindByIdAsync(id.ToString());
        if (role == null)
            return NotFound("Role not found.");

        if (!string.IsNullOrWhiteSpace(dto.RoleName) && dto.RoleName != role.Name)
        {
            role.Name = dto.RoleName;
            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
        }

        await _permissionService.AssignPermissionsToRole(role.Id, dto.Permissions);

        return Ok(new { message = "The group has been updated successfully." });
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(string id)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString());
        if (role == null)
            return NotFound("Role not found.");

        await _permissionService.RemovePermissionsFromRole(role.Id);

        var result = await _roleManager.DeleteAsync(role);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { message = "The group has been deleted successfully." });
    }


}
