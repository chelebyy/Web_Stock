using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.API.Attributes;
using Stock.API.Constants;
using Stock.Application.Common.Interfaces;
using Stock.Application.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Infrastructure.Data;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Stock.API.Controllers
{
    [ApiController]
    [Route(ApiConstants.ApiBaseRoute + "/[controller]")]
    [Authorize]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PermissionsController> _logger;

        public PermissionsController(
            IPermissionService permissionService,
            ApplicationDbContext context,
            ILogger<PermissionsController> logger)
        {
            _permissionService = permissionService;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<ActionResult<List<PermissionDto>>> GetAll()
        {
            var permissions = await _permissionService.GetAllPermissionsAsync();
            return Ok(permissions);
        }

        [HttpGet("groups")]
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<ActionResult<List<PermissionGroupDto>>> GetByGroups()
        {
            var permissionGroups = await _permissionService.GetPermissionsByGroupsAsync();
            return Ok(permissionGroups);
        }

        [HttpGet("role/{roleId}")]
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<ActionResult<List<PermissionDto>>> GetByRoleId(int roleId)
        {
            var permissions = await _permissionService.GetPermissionsByRoleIdAsync(roleId);
            return Ok(permissions);
        }

        [HttpGet("user/{userId}")]
        [RequirePermission(PermissionNames.PermissionsView)]
        public async Task<ActionResult<List<PermissionDto>>> GetByUserId(int userId)
        {
            var permissions = await _permissionService.GetPermissionsByUserIdAsync(userId);
            return Ok(permissions);
        }

        [HttpPost("role/{roleId}/assign")]
        [RequirePermission(PermissionNames.RolesUpdate)]
        public async Task<ActionResult<bool>> AssignPermissionsToRole(int roleId, [FromBody] AssignPermissionsRequest request)
        {
            var result = await _permissionService.AssignPermissionsToRoleAsync(roleId, request.PermissionIds);
            return Ok(result);
        }

        [HttpPost("user/{userId}/assign")]
        [RequirePermission(PermissionNames.UsersPermissionsAssign)]
        public async Task<ActionResult<bool>> AssignPermissionsToUser(int userId, [FromBody] AssignPermissionsRequest request)
        {
            try
            {
                _logger.LogInformation(string.Format(LogMessages.UserPermissionAssigning, userId, request.PermissionIds.Count));
                
                bool result = true;
                foreach (var permissionId in request.PermissionIds)
                {
                    var assignResult = await _permissionService.AssignPermissionToUserAsync(userId, permissionId, true);
                    if (!assignResult)
                    {
                        result = false;
                        _logger.LogWarning(string.Format(LogMessages.PermissionAssignFailed, userId, permissionId));
                    }
                }
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(LogMessages.UserPermissionAssignError, userId));
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = string.Format(ErrorMessages.UserPermissionAssignError, ex.Message) });
            }
        }

        [HttpPost("user/{userId}/assign/{permissionId}")]
        [RequirePermission(PermissionNames.UsersPermissionsAssign)]
        public async Task<ActionResult<bool>> AssignPermissionToUser(int userId, int permissionId, [FromQuery] bool isGranted = true)
        {
            var result = await _permissionService.AssignPermissionToUserAsync(userId, permissionId, isGranted);
            return Ok(result);
        }

        [HttpDelete("user/{userId}/permission/{permissionId}")]
        [RequirePermission(PermissionNames.UsersPermissionsRemove)]
        public async Task<ActionResult<bool>> RemoveUserPermission(int userId, int permissionId)
        {
            var result = await _permissionService.RemoveUserPermissionAsync(userId, permissionId);
            return Ok(result);
        }

        [HttpPost("user/{userId}/reset")]
        [RequirePermission(PermissionNames.UsersPermissionsReset)]
        public async Task<ActionResult<bool>> ResetUserToRolePermissions(int userId)
        {
            var result = await _permissionService.ResetUserToRolePermissionsAsync(userId);
            return Ok(result);
        }

        [HttpGet("user/{userId}/check/{permissionName}")]
        [RequirePermission(PermissionNames.PermissionsCheck)]
        public async Task<ActionResult<bool>> UserHasPermission(int userId, string permissionName)
        {
            var result = await _permissionService.UserHasPermissionAsync(userId, permissionName);
            return Ok(result);
        }

        [HttpGet("add-missing-permissions")]
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<ActionResult> AddMissingPermissionsManually()
        {
            try
            {
                _logger.LogInformation(LogMessages.AddingMissingPermissions);
                
                // Revir izni ekle
                var revirPermission = await _context.Permissions
                    .FirstOrDefaultAsync(p => p.Name == PermissionNames.PagesRevirView);
                
                if (revirPermission == null)
                {
                    _context.Permissions.Add(new Stock.Domain.Entities.Permissions.Permission
                    {
                        Name = PermissionNames.PagesRevirView,
                        Description = "Revir sayfasını görüntüleme",
                        Group = "Sayfa Erişimi",
                        ResourceType = "Page",
                        ResourceName = "Revir",
                        Action = "View",
                        CreatedAt = DateTime.UtcNow
                    });
                    
                    _logger.LogInformation(string.Format(LogMessages.PermissionAdded, PermissionNames.PagesRevirView));
                }
                else
                {
                    _logger.LogInformation(string.Format(LogMessages.PermissionAlreadyExists, PermissionNames.PagesRevirView));
                }
                
                // Bilgi İşlem izni ekle
                var bilgiIslemPermission = await _context.Permissions
                    .FirstOrDefaultAsync(p => p.Name == PermissionNames.PagesBilgiIslemView);
                
                if (bilgiIslemPermission == null)
                {
                    _context.Permissions.Add(new Stock.Domain.Entities.Permissions.Permission
                    {
                        Name = PermissionNames.PagesBilgiIslemView,
                        Description = "Bilgi İşlem sayfasını görüntüleme",
                        Group = "Sayfa Erişimi",
                        ResourceType = "Page",
                        ResourceName = "BilgiIslem",
                        Action = "View",
                        CreatedAt = DateTime.UtcNow
                    });
                    
                    _logger.LogInformation(string.Format(LogMessages.PermissionAdded, PermissionNames.PagesBilgiIslemView));
                }
                else
                {
                    _logger.LogInformation(string.Format(LogMessages.PermissionAlreadyExists, PermissionNames.PagesBilgiIslemView));
                }
                
                await _context.SaveChangesAsync();
                
                return Ok(new { message = ApiConstants.SuccessMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eksik izinler eklenirken bir hata oluştu.");
                return StatusCode(StatusCodes.Status500InternalServerError, string.Format(ErrorMessages.MissingPermissionsError, ex.Message));
            }
        }
    }

    public class AssignPermissionsRequest
    {
        public List<int> PermissionIds { get; set; } = new List<int>();
    }
} 