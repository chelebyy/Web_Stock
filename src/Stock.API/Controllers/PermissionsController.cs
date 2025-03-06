using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.API.Attributes;
using Stock.Application.Common.Interfaces;
using Stock.Application.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionsController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<PermissionDto>>> GetAll()
        {
            var permissions = await _permissionService.GetAllPermissionsAsync();
            return Ok(permissions);
        }

        [HttpGet("groups")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<PermissionGroupDto>>> GetByGroups()
        {
            var permissionGroups = await _permissionService.GetPermissionsByGroupsAsync();
            return Ok(permissionGroups);
        }

        [HttpGet("role/{roleId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<PermissionDto>>> GetByRoleId(int roleId)
        {
            var permissions = await _permissionService.GetPermissionsByRoleIdAsync(roleId);
            return Ok(permissions);
        }

        [HttpGet("user/{userId}")]
        [RequirePermission("Permissions.View")]
        public async Task<ActionResult<List<PermissionDto>>> GetByUserId(int userId)
        {
            var permissions = await _permissionService.GetPermissionsByUserIdAsync(userId);
            return Ok(permissions);
        }

        [HttpPost("role/{roleId}/assign")]
        [RequirePermission("Roles.Update")]
        public async Task<ActionResult<bool>> AssignPermissionsToRole(int roleId, [FromBody] AssignPermissionsRequest request)
        {
            var result = await _permissionService.AssignPermissionsToRoleAsync(roleId, request.PermissionIds);
            return Ok(result);
        }

        [HttpPost("user/{userId}/assign/{permissionId}")]
        [RequirePermission("Users.Permissions.Assign")]
        public async Task<ActionResult<bool>> AssignPermissionToUser(int userId, int permissionId, [FromQuery] bool isGranted = true)
        {
            var result = await _permissionService.AssignPermissionToUserAsync(userId, permissionId, isGranted);
            return Ok(result);
        }

        [HttpDelete("user/{userId}/permission/{permissionId}")]
        [RequirePermission("Users.Permissions.Remove")]
        public async Task<ActionResult<bool>> RemoveUserPermission(int userId, int permissionId)
        {
            var result = await _permissionService.RemoveUserPermissionAsync(userId, permissionId);
            return Ok(result);
        }

        [HttpPost("user/{userId}/reset")]
        [RequirePermission("Users.Permissions.Reset")]
        public async Task<ActionResult<bool>> ResetUserToRolePermissions(int userId)
        {
            var result = await _permissionService.ResetUserToRolePermissionsAsync(userId);
            return Ok(result);
        }

        [HttpGet("user/{userId}/check/{permissionName}")]
        [RequirePermission("Permissions.Check")]
        public async Task<ActionResult<bool>> UserHasPermission(int userId, string permissionName)
        {
            var result = await _permissionService.UserHasPermissionAsync(userId, permissionName);
            return Ok(result);
        }
    }

    public class AssignPermissionsRequest
    {
        public List<int> PermissionIds { get; set; } = new List<int>();
    }
} 