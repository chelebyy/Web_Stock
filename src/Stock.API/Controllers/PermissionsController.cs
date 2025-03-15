using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.API.Attributes;
using Stock.Application.Common.Interfaces;
using Stock.Application.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Infrastructure.Data;
using System.Linq;

namespace Stock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PermissionsController> _logger;

        // Sabit hata mesajları
        private const string ERROR_PERMISSION_ASSIGN = "Kullanıcıya izinler atanırken bir hata oluştu: {0}";
        private const string ERROR_MISSING_PERMISSIONS = "Eksik izinler eklenirken bir hata oluştu: {0}";
        private const string ADMIN_ROLE = "Admin";
        private const string PERMISSION_VIEW = "Permissions.View";
        private const string ROLES_UPDATE = "Roles.Update";
        private const string USERS_PERMISSIONS_ASSIGN = "Users.Permissions.Assign";
        private const string USERS_PERMISSIONS_REMOVE = "Users.Permissions.Remove";
        private const string USERS_PERMISSIONS_RESET = "Users.Permissions.Reset";
        private const string PERMISSIONS_CHECK = "Permissions.Check";
        private const string ERROR_PERMISSION_NOT_FOUND = "İzin bulunamadı.";
        private const string ERROR_USER_NOT_FOUND = "Kullanıcı bulunamadı.";
        private const string ERROR_ROLE_NOT_FOUND = "Rol bulunamadı.";
        private const string SUCCESS_PERMISSIONS_ADDED = "Eksik izinler başarıyla eklendi.";
        private const string PERMISSION_REVIR_VIEW = "Pages.Revir.View";
        private const string PERMISSION_BILGIISLEM_VIEW = "Pages.BilgiIslem.View";
        private const string PERMISSION_GROUP_PAGE_ACCESS = "Sayfa Erişimi";
        private const string RESOURCE_TYPE_PAGE = "Page";
        private const string ACTION_VIEW = "View";
        private const string RESOURCE_NAME_REVIR = "Revir";
        private const string RESOURCE_NAME_BILGIISLEM = "BilgiIslem";
        private const string LOG_PERMISSION_ADDED = "{0} izni eklendi";
        private const string LOG_ERROR_MISSING_PERMISSIONS = "Eksik izinler eklenirken bir hata oluştu";

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
        [Authorize(Roles = ADMIN_ROLE)]
        public async Task<ActionResult<List<PermissionDto>>> GetAll()
        {
            var permissions = await _permissionService.GetAllPermissionsAsync();
            return Ok(permissions);
        }

        [HttpGet("groups")]
        [Authorize(Roles = ADMIN_ROLE)]
        public async Task<ActionResult<List<PermissionGroupDto>>> GetByGroups()
        {
            var permissionGroups = await _permissionService.GetPermissionsByGroupsAsync();
            return Ok(permissionGroups);
        }

        [HttpGet("role/{roleId}")]
        [Authorize(Roles = ADMIN_ROLE)]
        public async Task<ActionResult<List<PermissionDto>>> GetByRoleId(int roleId)
        {
            var permissions = await _permissionService.GetPermissionsByRoleIdAsync(roleId);
            return Ok(permissions);
        }

        [HttpGet("user/{userId}")]
        [RequirePermission(PERMISSION_VIEW)]
        public async Task<ActionResult<List<PermissionDto>>> GetByUserId(int userId)
        {
            var permissions = await _permissionService.GetPermissionsByUserIdAsync(userId);
            return Ok(permissions);
        }

        [HttpPost("role/{roleId}/assign")]
        [RequirePermission(ROLES_UPDATE)]
        public async Task<ActionResult<bool>> AssignPermissionsToRole(int roleId, [FromBody] AssignPermissionsRequest request)
        {
            var result = await _permissionService.AssignPermissionsToRoleAsync(roleId, request.PermissionIds);
            return Ok(result);
        }

        [HttpPost("user/{userId}/assign")]
        [RequirePermission(USERS_PERMISSIONS_ASSIGN)]
        public async Task<ActionResult<bool>> AssignPermissionsToUser(int userId, [FromBody] AssignPermissionsRequest request)
        {
            try
            {
                _logger.LogInformation("Kullanıcıya izinler atanıyor - Kullanıcı ID: {UserId}, İzin sayısı: {PermissionCount}", userId, request.PermissionIds.Count);
                
                bool result = true;
                foreach (var permissionId in request.PermissionIds)
                {
                    var assignResult = await _permissionService.AssignPermissionToUserAsync(userId, permissionId, true);
                    if (!assignResult)
                    {
                        result = false;
                        _logger.LogWarning("İzin atanamadı - Kullanıcı ID: {UserId}, İzin ID: {PermissionId}", userId, permissionId);
                    }
                }
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcıya izinler atanırken hata oluştu - Kullanıcı ID: {UserId}", userId);
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = string.Format(ERROR_PERMISSION_ASSIGN, ex.Message) });
            }
        }

        [HttpPost("user/{userId}/assign/{permissionId}")]
        [RequirePermission(USERS_PERMISSIONS_ASSIGN)]
        public async Task<ActionResult<bool>> AssignPermissionToUser(int userId, int permissionId, [FromQuery] bool isGranted = true)
        {
            var result = await _permissionService.AssignPermissionToUserAsync(userId, permissionId, isGranted);
            return Ok(result);
        }

        [HttpDelete("user/{userId}/permission/{permissionId}")]
        [RequirePermission(USERS_PERMISSIONS_REMOVE)]
        public async Task<ActionResult<bool>> RemoveUserPermission(int userId, int permissionId)
        {
            var result = await _permissionService.RemoveUserPermissionAsync(userId, permissionId);
            return Ok(result);
        }

        [HttpPost("user/{userId}/reset")]
        [RequirePermission(USERS_PERMISSIONS_RESET)]
        public async Task<ActionResult<bool>> ResetUserToRolePermissions(int userId)
        {
            var result = await _permissionService.ResetUserToRolePermissionsAsync(userId);
            return Ok(result);
        }

        [HttpGet("user/{userId}/check/{permissionName}")]
        [RequirePermission(PERMISSIONS_CHECK)]
        public async Task<ActionResult<bool>> UserHasPermission(int userId, string permissionName)
        {
            var result = await _permissionService.UserHasPermissionAsync(userId, permissionName);
            return Ok(result);
        }

        [HttpGet("add-missing-permissions")]
        [Authorize(Roles = ADMIN_ROLE)]
        public async Task<ActionResult> AddMissingPermissionsManually()
        {
            try
            {
                // Revir izni ekle
                var revirPermission = await _context.Permissions
                    .FirstOrDefaultAsync(p => p.Name == PERMISSION_REVIR_VIEW);
                
                if (revirPermission == null)
                {
                    _context.Permissions.Add(new Stock.Domain.Entities.Permissions.Permission
                    {
                        Name = PERMISSION_REVIR_VIEW,
                        Description = "Revir sayfasını görüntüleme",
                        Group = PERMISSION_GROUP_PAGE_ACCESS,
                        ResourceType = RESOURCE_TYPE_PAGE,
                        ResourceName = RESOURCE_NAME_REVIR,
                        Action = ACTION_VIEW,
                        CreatedAt = DateTime.UtcNow
                    });
                    
                    _logger.LogInformation(string.Format(LOG_PERMISSION_ADDED, PERMISSION_REVIR_VIEW));
                }
                
                // Bilgi İşlem izni ekle
                var bilgiIslemPermission = await _context.Permissions
                    .FirstOrDefaultAsync(p => p.Name == PERMISSION_BILGIISLEM_VIEW);
                
                if (bilgiIslemPermission == null)
                {
                    _context.Permissions.Add(new Stock.Domain.Entities.Permissions.Permission
                    {
                        Name = PERMISSION_BILGIISLEM_VIEW,
                        Description = "Bilgi İşlem sayfasını görüntüleme",
                        Group = PERMISSION_GROUP_PAGE_ACCESS,
                        ResourceType = RESOURCE_TYPE_PAGE,
                        ResourceName = RESOURCE_NAME_BILGIISLEM,
                        Action = ACTION_VIEW,
                        CreatedAt = DateTime.UtcNow
                    });
                    
                    _logger.LogInformation(string.Format(LOG_PERMISSION_ADDED, PERMISSION_BILGIISLEM_VIEW));
                }
                
                await _context.SaveChangesAsync();
                
                return Ok(new { message = SUCCESS_PERMISSIONS_ADDED });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LOG_ERROR_MISSING_PERMISSIONS);
                return StatusCode(StatusCodes.Status500InternalServerError, string.Format(ERROR_MISSING_PERMISSIONS, ex.Message));
            }
        }
    }

    public class AssignPermissionsRequest
    {
        public List<int> PermissionIds { get; set; } = new List<int>();
    }
} 