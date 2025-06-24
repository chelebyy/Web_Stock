using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.API.Attributes;
using Stock.Application.Constants;
using Stock.Application.Common.Interfaces;
using Stock.Application.Models.Common;
using Stock.Application.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities.Permissions;
using Stock.Infrastructure.Data;
using System.Linq;
using Microsoft.AspNetCore.Http;
using MediatR;
using Stock.Application.Features.Permissions.Queries.GetAllPermissions;
using Stock.Application.Features.Permissions.Commands;
using Stock.Application.Features.Permissions.Queries.GetPermissionById;
using Stock.Application.Features.Permissions.Queries.GetPermissionsByGroups;
using Stock.Application.Features.Permissions.Queries.GetPermissionsByRoleId;
using Stock.Application.Features.Permissions.Queries.GetPermissionsByUserId;
using Stock.Application.Features.Users.Commands.UpdateUserPermissions;
using Stock.Domain.Exceptions;

namespace Stock.API.Controllers
{
    /// <summary>
    /// İzin yönetimi ile ilgili API endpoint'lerini içerir (CQRS ile).
    /// İzinleri listeleme, gruplama, role veya kullanıcıya atama, kaldırma ve kontrol etme işlemlerini sağlar.
    /// </summary>
    [ApiController]
    [Route(ApiConstants.ApiBaseRoute + "/[controller]")]
    [Authorize]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PermissionsController> _logger;
        private readonly IMediator _mediator;

        /// <summary>
        /// PermissionsController sınıfının yapıcı metodu.
        /// Bağımlılıkları (IPermissionService, ApplicationDbContext, ILogger, IMediator) enjekte eder.
        /// </summary>
        /// <param name="permissionService">İzin işlemleri servisi.</param>
        /// <param name="context">Veritabanı context nesnesi (Geçici olarak kullanılıyor).</param>
        /// <param name="logger">Loglama servisi.</param>
        /// <param name="mediator">MediatR nesnesi.</param>
        public PermissionsController(
            IPermissionService permissionService,
            ApplicationDbContext context,
            ILogger<PermissionsController> logger,
            IMediator mediator)
        {
            _permissionService = permissionService;
            _context = context;
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Sistemdeki tüm tanımlı izinleri listeler.
        /// </summary>
        /// <returns>İzinlerin listesi.</returns>
        /// <response code="200">İzinler başarıyla listelendi.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PermissionDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PermissionDto>>> GetAllPermissions()
        {
            var query = new GetAllPermissionsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets a specific permission by its ID.
        /// </summary>
        /// <param name="id">The ID of the permission.</param>
        /// <returns>The permission DTO.</returns>
        [HttpGet("{id}")]
        [RequirePermission(PermissionNames.PermissionsView)]
        [ProducesResponseType(typeof(PermissionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PermissionDto>> GetPermissionById(int id)
        {
            var query = new GetPermissionByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Sistemdeki tüm izinleri gruplarına göre listeler (Sadece Admin yetkisine sahip kullanıcılar).
        /// </summary>
        /// <returns>A list of permission groups with their permissions.</returns>
        [HttpGet("groups")]
        [RequirePermission(PermissionNames.PermissionsView)]
        [ProducesResponseType(typeof(List<PermissionGroupDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PermissionGroupDto>>> GetByGroups()
        {
            _logger.LogInformation("Getting all permissions grouped by their group name.");
            var query = new GetPermissionsByGroupsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Belirtilen role atanmış izinleri listeler (Sadece Admin yetkisine sahip kullanıcılar).
        /// </summary>
        /// <param name="roleId">The ID of the role.</param>
        /// <returns>A list of permissions assigned to the role.</returns>
        [HttpGet("role/{roleId}")]
        [RequirePermission(PermissionNames.PermissionsView)]
        [ProducesResponseType(typeof(List<PermissionDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<PermissionDto>>> GetByRoleId(int roleId)
        {
            _logger.LogInformation("Getting permissions for role with ID: {RoleId}", roleId);
            var query = new GetPermissionsByRoleIdQuery(roleId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Belirtilen kullanıcıya atanmış (doğrudan veya rolünden gelen) izinleri listeler.
        /// Bu endpoint'e erişim için 'Permissions.View' izni gereklidir.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of permissions assigned to the user.</returns>
        [HttpGet("user/{userId}")]
        [RequirePermission(PermissionNames.PermissionsView)]
        [ProducesResponseType(typeof(List<PermissionDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<PermissionDto>>> GetByUserId(int userId)
        {
            _logger.LogInformation("Getting all permissions for user with ID: {UserId}", userId);
            var query = new GetPermissionsByUserIdQuery(userId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Bir kullanıcıya ait özel izinleri günceller.
        /// </summary>
        /// <param name="userId">İzinleri güncellenecek kullanıcının ID'si.</param>
        /// <param name="command">Yeni izinleri ve durumlarını (granted/revoked) içeren komut.</param>
        /// <returns>Başarılı olursa NoContent (204) yanıtı döner.</returns>
        /// <response code="204">Kullanıcı izinleri başarıyla güncellendi.</response>
        /// <response code="400">Geçersiz kullanıcı ID'si veya izin bilgileri.</response>
        /// <response code="404">Belirtilen ID'ye sahip kullanıcı bulunamadı.</response>
        [HttpPut("users/{userId}/permissions")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserPermissions(int userId, [FromBody] UpdateUserPermissionsCommand command)
        {
            _logger.LogInformation($"Attempting to update permissions for user with id: {userId}");

            command.UserId = userId;

            try
            {
                await _mediator.Send(command);
                _logger.LogInformation($"Permissions for user with id: {userId} updated successfully.");
                return NoContent();
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, $"Validation error while updating permissions for user: {userId}");
                var errors = new Dictionary<string, string[]>(ex.Errors);
                return BadRequest(new ValidationErrorResponse(StatusCodes.Status400BadRequest, ex.Message, errors));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, $"User with id: {userId} not found for permission update.");
                return NotFound(new ErrorResponse(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating permissions for user: {userId}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(StatusCodes.Status500InternalServerError, "An unexpected error occurred."));
            }
        }

        /// <summary>
        /// Belirtilen kullanıcının belirtilen izne sahip olup olmadığını kontrol eder.
        /// Bu endpoint'e erişim için 'Permissions.Check' izni gereklidir.
        /// </summary>
        /// <param name="userId">Kontrol edilecek kullanıcının ID'si.</param>
        /// <param name="permissionName">Kontrol edilecek iznin adı (örn. "Users.View").</param>
        /// <returns>Kullanıcı izne sahipse true, değilse false döner.</returns>
        /// <response code="200">İzin kontrol sonucu başarıyla döndürüldü.</response>
        /// <response code="401">Kimlik doğrulanmamış veya token geçersiz.</response>
        /// <response code="403">Gerekli 'Permissions.Check' izni bulunmuyor.</response>
        /// <response code="404">Kullanıcı ID veya izin adı bulunamadı.</response>
        /// <response code="500">İzin kontrolü sırasında bir sunucu hatası oluştu.</response>
        [HttpGet("user/{userId}/check/{permissionName}")]
        [RequirePermission(PermissionNames.PermissionsCheck)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> UserHasPermission(int userId, string permissionName)
        {
            var result = await _permissionService.UserHasPermissionAsync(userId, permissionName);
            return Ok(result);
        }

        /// <summary>
        /// Veritabanına eksik olan temel sayfa erişim izinlerini manuel olarak ekler (Sadece Admin).
        /// Bu endpoint genellikle ilk kurulum veya güncelleme sonrası kullanılır.
        /// </summary>
        /// <remarks>
        /// Bu metod, `PermissionNames` sınıfında tanımlı olan ancak veritabanında bulunmayan
        /// `Pages.Revir.View` ve `Pages.BilgiIslem.View` izinlerini ekler.
        /// Bu endpoint'in production ortamında dikkatli kullanılması gerekir.
        /// TODO: Bu işlem startup'ta veya migration içinde yapılmalı.
        /// </remarks>
        /// <returns>Başarılı olursa Ok mesajı döner.</returns>
        /// <response code="200">Eksik izinler başarıyla kontrol edildi ve/veya eklendi.</response>
        /// <response code="401">Yetkisiz erişim (Admin rolü gerekli).</response>
        /// <response code="500">İzin ekleme sırasında bir sunucu hatası oluştu.</response>
        [HttpGet("add-missing-permissions")]
        [Authorize(Roles = RoleNames.Admin)]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddMissingPermissionsManually()
        {
            try
            {
                _logger.LogInformation(LogMessages.AddingMissingPermissions);
                
                await AddPermissionIfNotExistsAsync(PermissionNames.PagesRevirView, "Revir sayfasını görüntüleme", "Sayfa Erişimi", "Page", "Revir", "View");
                
                await AddPermissionIfNotExistsAsync(PermissionNames.PagesBilgiIslemView, "Bilgi İşlem sayfasını görüntüleme", "Sayfa Erişimi", "Page", "BilgiIslem", "View");
                
                await _context.SaveChangesAsync();
                
                return Ok(new { message = ApiConstants.SuccessMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorAddingMissingPermissions);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(StatusCodes.Status500InternalServerError, string.Format(ErrorMessages.MissingPermissionsError, ex.Message)));
            }
        }

        private async Task AddPermissionIfNotExistsAsync(string name, string description, string group, string resourceType, string resourceName, string action)
        {
            var permissionExists = await _context.Permissions.AnyAsync(p => p.Name == name);
            if (!permissionExists)
            {
                var permissionResult = Permission.Create(
                    name: name,
                    description: description,
                    resourceType: resourceType,
                    resourceName: resourceName,
                    action: action,
                    group: group);

                if (permissionResult.IsSuccess)
                {
                    _context.Permissions.Add(permissionResult.Value);
                    _logger.LogInformation(string.Format(LogMessages.PermissionAdded, name));
                }
                else
                {
                    _logger.LogWarning(string.Format("İzin eklenemedi: {0}. Hata: {1}", name, permissionResult.Error));
                }
            }
            else
            {
                _logger.LogInformation(string.Format(LogMessages.PermissionAlreadyExists, name));
            }
        }

        /// <summary>
        /// Yeni bir izin oluşturur.
        /// Bu endpoint'e erişim için 'Permissions.Create' izni gereklidir.
        /// </summary>
        /// <param name="command">Oluşturulacak iznin bilgilerini içeren komut.</param>
        /// <returns>Oluşturulan iznin bilgileri.</returns>
        /// <response code="201">İzin başarıyla oluşturuldu ve oluşturulan izin bilgileri döndürüldü.</response>
        /// <response code="400">Geçersiz istek verisi veya domain doğrulama hatası.</response>
        /// <response code="401">Kimlik doğrulanmamış veya token geçersiz.</response>
        /// <response code="403">Gerekli 'Permissions.Create' izni bulunmuyor.</response>
        /// <response code="500">İzin oluşturma sırasında bir sunucu hatası oluştu.</response>
        [HttpPost]
        [RequirePermission(PermissionNames.PermissionsCreate)]
        [ProducesResponseType(typeof(PermissionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PermissionDto>> CreatePermission([FromBody] CreatePermissionCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("İzin oluşturma başarısız: {Error}", result.Error);
                    return BadRequest(result.Error); // veya Problem detayları ile
                }

                // Başarılı durumda 201 Created yanıtı döndür
                // Normalde GetById endpoint'i olmalı, ancak burada sadece DTO dönüyoruz.
                // return CreatedAtAction(nameof(GetPermissionById), new { id = result.Value.Id }, result.Value);
                return StatusCode(StatusCodes.Status201Created, result.Value);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, $"Validation error while creating permission: {command.Name}");
                var errors = new Dictionary<string, string[]>(ex.Errors);
                return BadRequest(new ValidationErrorResponse(StatusCodes.Status400BadRequest, ex.Message, errors));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating permission: {command.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(StatusCodes.Status500InternalServerError, "An unexpected error occurred."));
            }
        }

        /// <summary>
        /// Mevcut bir izni günceller.
        /// Bu endpoint'e erişim için 'Permissions.Update' izni gereklidir.
        /// </summary>
        /// <param name="id">Güncellenecek iznin ID'si.</param>
        /// <param name="command">Güncellenecek iznin bilgilerini içeren komut.</param>
        /// <returns>İşlem başarılı olursa 204 No Content döner.</returns>
        /// <response code="204">İzin başarıyla güncellendi.</response>
        /// <response code="400">Geçersiz istek verisi, ID uyuşmazlığı veya domain doğrulama hatası.</response>
        /// <response code="401">Kimlik doğrulanmamış veya token geçersiz.</response>
        /// <response code="403">Gerekli 'Permissions.Update' izni bulunmuyor.</response>
        /// <response code="404">Güncellenecek izin bulunamadı.</response>
        /// <response code="500">İzin güncelleme sırasında bir sunucu hatası oluştu.</response>
        [HttpPut("{id}")]
        [RequirePermission(PermissionNames.PermissionsUpdate)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePermission(int id, [FromBody] UpdatePermissionCommand command)
        {
            if (id != command.Id)
            {
                _logger.LogWarning("İzin güncelleme isteğinde ID uyuşmazlığı: Route ID {RouteId}, Command ID {CommandId}", id, command.Id);
                return BadRequest("ID mismatch between route and command.");
            }

            try
            {
                var result = await _mediator.Send(command);

                if (!result.IsSuccess)
                {
                    // Hata mesajına göre NotFound veya BadRequest döndür
                    if (result.Error.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    {
                         _logger.LogWarning("Güncellenecek izin bulunamadı: ID {PermissionId}", command.Id);
                        return NotFound(result.Error);
                    }
                    _logger.LogWarning("İzin güncelleme başarısız: {Error}", result.Error);
                    return BadRequest(result.Error);
                }

                return NoContent(); // Başarılı güncellemede 204 No Content
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, $"Validation error while updating permission: {id}");
                var errors = new Dictionary<string, string[]>(ex.Errors);
                return BadRequest(new ValidationErrorResponse(StatusCodes.Status400BadRequest, ex.Message, errors));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, $"Permission with id: {id} not found for update.");
                return NotFound(new ErrorResponse(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating permission: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(StatusCodes.Status500InternalServerError, "An unexpected error occurred."));
            }
        }

        /// <summary>
        /// Belirtilen ID'ye sahip izni siler.
        /// Bu endpoint'e erişim için 'Permissions.Delete' izni gereklidir.
        /// </summary>
        /// <param name="id">Silinecek iznin ID'si.</param>
        /// <returns>İşlem başarılı olursa 204 No Content döner.</returns>
        /// <response code="204">İzin başarıyla silindi.</response>
        /// <response code="401">Kimlik doğrulanmamış veya token geçersiz.</response>
        /// <response code="403">Gerekli 'Permissions.Delete' izni bulunmuyor.</response>
        /// <response code="404">Silinecek izin bulunamadı.</response>
        /// <response code="500">İzin silme sırasında bir sunucu hatası oluştu.</response>
        [HttpDelete("{id}")]
        [RequirePermission(PermissionNames.PermissionsDelete)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePermission(int id)
        {
            var command = new DeletePermissionCommand(id);
            try
            {
                var result = await _mediator.Send(command);

                if (!result.IsSuccess)
                {
                     // Hata mesajına göre NotFound veya BadRequest döndür (Genellikle not found olur)
                    if (result.Error.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    {
                        _logger.LogWarning("Silinecek izin bulunamadı: ID {PermissionId}", id);
                        return NotFound(result.Error);
                    }
                    _logger.LogWarning("İzin silme başarısız: {Error}", result.Error);
                    // Silme işleminde genelde 404 döneriz, 400 nadir olur.
                    return NotFound(result.Error); 
                }

                return NoContent(); // Başarılı silmede 204 No Content
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, $"Permission with id: {id} not found for delete.");
                return NotFound(new ErrorResponse(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting permission: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(StatusCodes.Status500InternalServerError, "An unexpected error occurred."));
            }
        }

        [HttpDelete("users/{userId}/permissions/{permissionId}")]
        public async Task<IActionResult> RemoveUserPermission(int userId, int permissionId)
        {
            // This can be refactored to a command later if needed
            var result = await _permissionService.RemoveUserPermissionAsync(userId, permissionId);
            if (result)
            {
                return Ok(new { message = "Kullanıcı izni başarıyla kaldırıldı." });
            }
            return BadRequest(new { error = "Kullanıcı izni kaldırılamadı." });
        }

        [HttpPost("users/{userId}/reset-permissions")]
        public async Task<IActionResult> ResetUserPermissions(int userId)
        {
            // This can be refactored to a command later if needed
             var result = await _permissionService.ResetUserToRolePermissionsAsync(userId);
            if (result)
            {
                return Ok(new { message = "Kullanıcı izinleri başarıyla rol varsayılanlarına sıfırlandı." });
            }
            return BadRequest(new { error = "Kullanıcı izinleri sıfırlanamadı." });
        }
    }

    /// <summary>
    /// Kullanıcıya veya role toplu izin atama isteği için kullanılan DTO.
    /// </summary>
    public class AssignPermissionsRequest
    {
        /// <summary>
        /// Atanacak izinlerin ID listesi.
        /// </summary>
        public List<int> PermissionIds { get; set; } = new List<int>();
    }
} 