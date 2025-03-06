using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.API.Attributes;
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

        [HttpPost("user/{userId}/assign")]
        [RequirePermission("Users.Permissions.Assign")]
        public async Task<ActionResult<bool>> AssignPermissionsToUser(int userId, [FromBody] AssignPermissionsRequest request)
        {
            try
            {
                _logger.LogInformation($"Kullanıcıya izinler atanıyor - Kullanıcı ID: {userId}, İzin sayısı: {request.PermissionIds.Count}");
                
                bool result = true;
                foreach (var permissionId in request.PermissionIds)
                {
                    var assignResult = await _permissionService.AssignPermissionToUserAsync(userId, permissionId, true);
                    if (!assignResult)
                    {
                        result = false;
                        _logger.LogWarning($"İzin atanamadı - Kullanıcı ID: {userId}, İzin ID: {permissionId}");
                    }
                }
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Kullanıcıya izinler atanırken hata oluştu - Kullanıcı ID: {userId}");
                return StatusCode(500, new { error = "Kullanıcıya izinler atanırken bir hata oluştu: " + ex.Message });
            }
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

        [HttpGet("add-missing-permissions")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddMissingPermissionsManually()
        {
            try
            {
                _logger.LogInformation("Eksik izinler ekleniyor...");
                
                // Revir izni ekle
                var revirPermission = await _context.Permissions
                    .FirstOrDefaultAsync(p => p.Name == "Pages.Revir.View");
                
                if (revirPermission == null)
                {
                    _context.Permissions.Add(new Permission
                    {
                        Name = "Pages.Revir.View",
                        Description = "Revir sayfasını görüntüleme",
                        Group = "Sayfa Erişimi",
                        ResourceType = "Page",
                        ResourceName = "Revir",
                        Action = "View",
                        CreatedAt = DateTime.UtcNow
                    });
                    
                    _logger.LogInformation("Pages.Revir.View izni eklendi");
                }
                else
                {
                    _logger.LogInformation("Pages.Revir.View izni zaten mevcut");
                }
                
                // BilgiIslem izni ekle
                var bilgiIslemPermission = await _context.Permissions
                    .FirstOrDefaultAsync(p => p.Name == "Pages.BilgiIslem.View");
                
                if (bilgiIslemPermission == null)
                {
                    _context.Permissions.Add(new Permission
                    {
                        Name = "Pages.BilgiIslem.View",
                        Description = "Bilgi İşlem sayfasını görüntüleme",
                        Group = "Sayfa Erişimi",
                        ResourceType = "Page",
                        ResourceName = "BilgiIslem",
                        Action = "View",
                        CreatedAt = DateTime.UtcNow
                    });
                    
                    _logger.LogInformation("Pages.BilgiIslem.View izni eklendi");
                }
                else
                {
                    _logger.LogInformation("Pages.BilgiIslem.View izni zaten mevcut");
                }
                
                await _context.SaveChangesAsync();
                
                return Ok(new { message = "Eksik izinler başarıyla eklendi" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eksik izinler eklenirken bir hata oluştu.");
                return StatusCode(500, "Eksik izinler eklenirken bir hata oluştu: " + ex.Message);
            }
        }
    }

    public class AssignPermissionsRequest
    {
        public List<int> PermissionIds { get; set; } = new List<int>();
    }
} 