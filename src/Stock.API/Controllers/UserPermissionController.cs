using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Interfaces;
using Stock.Application.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stock.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserPermissionController : ControllerBase
    {
        private readonly IUserPermissionService _userPermissionService;
        private readonly ILogger<UserPermissionController> _logger;

        public UserPermissionController(
            IUserPermissionService userPermissionService,
            ILogger<UserPermissionController> logger)
        {
            _userPermissionService = userPermissionService;
            _logger = logger;
        }

        [HttpGet("user/{userId}")]
        [Authorize(Policy = "PermissionRequirement")]
        public async Task<ActionResult<List<PermissionDto>>> GetUserPermissions(int userId)
        {
            try
            {
                var permissions = await _userPermissionService.GetUserPermissionsAsync(userId);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı izinleri alınırken hata oluştu. Kullanıcı ID: {UserId}", userId);
                return StatusCode(500, "Kullanıcı izinleri alınırken bir hata oluştu.");
            }
        }

        [HttpGet("user/{userId}/custom")]
        [Authorize(Policy = "PermissionRequirement")]
        public async Task<ActionResult<List<UserPermissionDto>>> GetUserCustomPermissions(int userId)
        {
            try
            {
                var customPermissions = await _userPermissionService.GetUserCustomPermissionsAsync(userId);
                return Ok(customPermissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı özel izinleri alınırken hata oluştu. Kullanıcı ID: {UserId}", userId);
                return StatusCode(500, "Kullanıcı özel izinleri alınırken bir hata oluştu.");
            }
        }

        [HttpPost("user/{userId}/permission/{permissionId}")]
        [Authorize(Policy = "PermissionRequirement")]
        public async Task<ActionResult> AssignPermissionToUser(int userId, int permissionId, [FromQuery] bool isGranted = true)
        {
            try
            {
                var result = await _userPermissionService.AssignPermissionToUserAsync(userId, permissionId, isGranted);
                if (!result)
                {
                    return BadRequest("İzin kullanıcıya atanamadı.");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İzin kullanıcıya atanırken hata oluştu. Kullanıcı ID: {UserId}, İzin ID: {PermissionId}", userId, permissionId);
                return StatusCode(500, "İzin kullanıcıya atanırken bir hata oluştu.");
            }
        }

        [HttpDelete("user/{userId}/permission/{permissionId}")]
        [Authorize(Policy = "PermissionRequirement")]
        public async Task<ActionResult> RemoveUserPermission(int userId, int permissionId)
        {
            try
            {
                var result = await _userPermissionService.RemoveUserPermissionAsync(userId, permissionId);
                if (!result)
                {
                    return BadRequest("İzin kullanıcıdan kaldırılamadı.");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İzin kullanıcıdan kaldırılırken hata oluştu. Kullanıcı ID: {UserId}, İzin ID: {PermissionId}", userId, permissionId);
                return StatusCode(500, "İzin kullanıcıdan kaldırılırken bir hata oluştu.");
            }
        }

        [HttpPost("user/{userId}/reset")]
        [Authorize(Policy = "PermissionRequirement")]
        public async Task<ActionResult> ResetUserToRolePermissions(int userId)
        {
            try
            {
                var result = await _userPermissionService.ResetUserToRolePermissionsAsync(userId);
                if (!result)
                {
                    return BadRequest("Kullanıcı izinleri rol izinlerine sıfırlanamadı.");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı izinleri rol izinlerine sıfırlanırken hata oluştu. Kullanıcı ID: {UserId}", userId);
                return StatusCode(500, "Kullanıcı izinleri rol izinlerine sıfırlanırken bir hata oluştu.");
            }
        }

        [HttpGet("user/{userId}/check/{permissionName}")]
        public async Task<ActionResult<bool>> HasPermission(int userId, string permissionName)
        {
            try
            {
                var hasPermission = await _userPermissionService.HasPermissionAsync(userId, permissionName);
                return Ok(hasPermission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İzin kontrolü yapılırken hata oluştu. Kullanıcı ID: {UserId}, İzin Adı: {PermissionName}", userId, permissionName);
                return StatusCode(500, "İzin kontrolü yapılırken bir hata oluştu.");
            }
        }
    }
} 