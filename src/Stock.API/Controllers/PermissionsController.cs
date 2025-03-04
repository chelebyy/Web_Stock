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

        [HttpPost("role/{roleId}/assign")]
        [Authorize(Roles = "Admin")]
        [RequirePermission("Roles.Update")]
        public async Task<ActionResult<bool>> AssignPermissionsToRole(int roleId, [FromBody] AssignPermissionsRequest request)
        {
            var result = await _permissionService.AssignPermissionsToRoleAsync(roleId, request.PermissionIds);
            return Ok(result);
        }
    }

    public class AssignPermissionsRequest
    {
        public List<int> PermissionIds { get; set; } = new List<int>();
    }
} 