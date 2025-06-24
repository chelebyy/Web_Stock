using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stock.Application.Models.Common;
using Stock.Application.Models;
using Stock.Application.Features.Roles.Commands.CreateRole;
using Stock.Application.Features.Roles.Commands.DeleteRole;
using Stock.Application.Features.Roles.Commands.UpdateRole;
using Stock.Application.Features.Roles.Commands.UpdateRolePermissions;
using Stock.Application.Features.Roles.Queries.GetAllRoles;
using Stock.Application.Features.Roles.Queries.GetRoleById;
using Stock.Application.Models.DTOs;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Linq;

namespace Stock.API.Controllers
{
    /// <summary>
    /// Rol yönetimi ile ilgili API endpoint'lerini içerir (CQRS ile).
    /// Rolleri listeleme, detaylarını görme, oluşturma, güncelleme ve silme işlemlerini sağlar.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] // Rol bazlı yetkilendirme
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RolesController> _logger;

        /// <summary>
        /// RolesController sınıfının yapıcı metodu.
        /// IMediator ve ILogger bağımlılıklarını enjekte eder.
        /// </summary>
        /// <param name="mediator">MediatR nesnesi.</param>
        /// <param name="logger">ILogger<RolesController> nesnesi.</param>
        public RolesController(IMediator mediator, ILogger<RolesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Sistemdeki tüm rolleri listeler.
        /// </summary>
        /// <returns>Rollerin listesi (Id, Name, Description, UserCount vb.).</returns>
        /// <response code="200">Roller başarıyla listelendi.</response>
        /// <response code="500">Roller listelenirken bir sunucu hatası oluştu.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<RoleSlimDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PagedResponse<RoleSlimDto>>> GetAllRoles([FromQuery] GetAllRolesQuery query)
        {
            _logger.LogInformation("Fetching all roles.");
            var roles = await _mediator.Send(query);
            return Ok(roles);
        }

        /// <summary>
        /// Belirtilen ID'ye sahip rolün detaylarını getirir.
        /// </summary>
        /// <param name="id">Detayları getirilecek rolün ID'si.</param>
        /// <returns>Rol detayları (Id, Name, Description, UserCount, Users listesi vb.).</returns>
        /// <response code="200">Rol detayları başarıyla getirildi.</response>
        /// <response code="404">Belirtilen ID'ye sahip rol bulunamadı.</response>
        /// <response code="500">Rol detayları getirilirken bir sunucu hatası oluştu.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RoleDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<RoleDto>> GetRoleById(int id)
        {
            _logger.LogInformation($"Fetching role with id: {id}");
            var query = new GetRoleByIdQuery(id);
            var role = await _mediator.Send(query);

            if (role == null)
            {
                _logger.LogWarning($"Role with id: {id} not found.");
                return NotFound();
            }
            return Ok(role);
        }

        /// <summary>
        /// Yeni bir rol oluşturur.
        /// </summary>
        /// <param name="command">Oluşturulacak rol bilgilerini içeren komut.</param>
        /// <returns>Oluşturulan rolün bilgileri.</returns>
        /// <response code="201">Rol başarıyla oluşturuldu. Oluşturulan rolün detaylarını döner.</response>
        /// <response code="400">Geçersiz rol bilgileri gönderildi (Validation Error).</response>
        /// <response code="409">Aynı isimde başka bir rol zaten mevcut (Conflict Error).</response>
        [HttpPost]
        [ProducesResponseType(typeof(RoleDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<RoleDto>> CreateRole([FromBody] CreateRoleCommand command)
        {
            _logger.LogInformation($"Attempting to create role: {command.Name}");
            try
            {
                var roleDto = await _mediator.Send(command);
                _logger.LogInformation($"Role created successfully: {roleDto.Name}, ID: {roleDto.Id}");
                return CreatedAtAction(nameof(GetRoleById), new { id = roleDto.Id }, roleDto);
            }
            catch (Stock.Domain.Exceptions.ValidationException ex)
            {
                _logger.LogWarning(ex, $"Validation error while creating role: {command.Name}");
                var errors = new Dictionary<string, string[]>(ex.Errors);
                return BadRequest(new ErrorResponse { Message = ex.Message, Errors = errors });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error creating role: {command.Name}");
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse { Message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Mevcut bir rolü günceller.
        /// </summary>
        /// <param name="id">Güncellenecek rolün ID'si.</param>
        /// <param name="command">Güncellenmiş rol bilgilerini içeren komut.</param>
        /// <returns>Başarılı olursa NoContent (204) yanıtı döner.</returns>
        /// <response code="204">Rol başarıyla güncellendi.</response>
        /// <response code="400">Geçersiz rol bilgileri veya ID uyuşmazlığı.</response>
        /// <response code="404">Belirtilen ID'ye sahip rol bulunamadı.</response>
        /// <response code="409">Aynı isimde başka bir rol zaten mevcut.</response>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRoleCommand command)
        {
            _logger.LogInformation($"Attempting to update role with id: {id}");
            if (id != command.Id)
            {
                _logger.LogWarning($"Mismatched id in route ({id}) and body ({command.Id}) for role update.");
                return BadRequest(new ErrorResponse { Message = "ID mismatch between route and body." });
            }

            try
            {
                await _mediator.Send(command);
                _logger.LogInformation($"Role with id: {id} updated successfully.");
                return NoContent();
            }
            catch (Stock.Domain.Exceptions.ValidationException ex)
            {
                _logger.LogWarning(ex, $"Validation error while updating role: {id}");
                var errors = new Dictionary<string, string[]>(ex.Errors);
                return BadRequest(new ErrorResponse { Message = ex.Message, Errors = errors });
            }
            catch (Stock.Domain.Exceptions.NotFoundException ex)
            {
                _logger.LogWarning(ex, $"Role with id: {id} not found for update.");
                return NotFound(new ErrorResponse { Message = ex.Message });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error updating role: {id}");
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse { Message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Bir role ait izinleri günceller. Mevcut tüm izinleri siler ve yenilerini ekler.
        /// </summary>
        /// <param name="id">İzinleri güncellenecek rolün ID'si.</param>
        /// <param name="command">Yeni izin ID'lerini içeren komut.</param>
        /// <returns>Başarılı olursa NoContent (204) yanıtı döner.</returns>
        /// <response code="204">Rol izinleri başarıyla güncellendi.</response>
        /// <response code="400">Geçersiz rol ID'si veya izin bilgileri.</response>
        /// <response code="404">Belirtilen ID'ye sahip rol bulunamadı.</response>
        [HttpPut("{id}/permissions")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateRolePermissions(int id, [FromBody] UpdateRolePermissionsCommand command)
        {
            _logger.LogInformation($"Attempting to update permissions for role with id: {id}");
            
            command.RoleId = id;

            try
            {
                await _mediator.Send(command);
                _logger.LogInformation($"Permissions for role with id: {id} updated successfully.");
                return NoContent();
            }
            catch (Stock.Domain.Exceptions.ValidationException ex)
            {
                _logger.LogWarning(ex, $"Validation error while updating permissions for role: {id}");
                var errors = new Dictionary<string, string[]>(ex.Errors);
                return BadRequest(new ErrorResponse { Message = ex.Message, Errors = errors });
            }
            catch (Stock.Domain.Exceptions.NotFoundException ex)
            {
                _logger.LogWarning(ex, $"Role with id: {id} not found for permission update.");
                return NotFound(new ErrorResponse { Message = ex.Message });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error updating permissions for role: {id}");
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse { Message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Belirtilen ID'ye sahip rolü siler.
        /// </summary>
        /// <param name="id">Silinecek rolün ID'si.</param>
        /// <returns>Başarılı olursa NoContent (204) yanıtı döner.</returns>
        /// <response code="204">Rol başarıyla silindi.</response>
        /// <response code="404">Belirtilen ID'ye sahip rol bulunamadı.</response>
        /// <response code="400">Rol silinemedi (örn. kullanılıyor).</response>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteRole(int id)
        {
            _logger.LogInformation($"Attempting to delete role with id: {id}");
            try
            {
                await _mediator.Send(new DeleteRoleCommand(id));
                _logger.LogInformation($"Role with id: {id} deleted successfully.");
                return NoContent();
            }
            catch (Stock.Domain.Exceptions.NotFoundException ex)
            {
                _logger.LogWarning(ex, $"Role with id: {id} not found for delete.");
                return NotFound(new ErrorResponse { Message = ex.Message });
            }
            catch (Stock.Domain.Exceptions.ConflictException ex)
            {
                _logger.LogWarning(ex, $"Attempted to delete role with id: {id} but it is in use.");
                return BadRequest(new ErrorResponse { Message = ex.Message });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error deleting role: {id}");
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse { Message = "An unexpected error occurred." });
            }
        }
    }
} 