using HRManagementSystem.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class PermissionsController : ControllerBase
{
    private readonly IPermissionService _permissionService;

    public PermissionsController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }


    [HttpGet("all")]
    public async Task<IActionResult> GetAllPermissions()
    {
        var permissions = await _permissionService.GetAllPermissionsAsync();
        return Ok(permissions);
    }
}
