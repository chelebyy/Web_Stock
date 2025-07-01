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
using Stock.Application.Features.Permissions.Queries.GetUserHasPermission;
using Stock.Application.Features.Permissions.Commands.AddMissingPermissions;
using Stock.Application.Features.Permissions.Commands.RemoveUserPermission;
using Stock.Application.Features.Permissions.Commands.ResetUserPermissions;
using Stock.Application.Features.Permissions.Queries.GetUserCustomPermissions;


namespace Stock.API.Controllers
{
    /// <summary>
    /// Provides API endpoints for managing permissions using the CQRS pattern.
    /// Allows for listing, grouping, assigning to roles/users, revoking, and checking permissions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PermissionsController : ControllerBase
    {
        private readonly ILogger<PermissionsController> _logger;
        private readonly IMediator _mediator;

        /// <summary>
        /// PermissionsController sınıfının yapıcı metodu.
        /// Bağımlılıkları (ILogger, IMediator) enjekte eder.
        /// </summary>
        /// <param name="logger">Loglama servisi.</param>
        /// <param name="mediator">MediatR nesnesi.</param>
        public PermissionsController(
            ILogger<PermissionsController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves a list of all defined permissions in the system.
        /// </summary>
        /// <returns>A list of permissions.</returns>
        /// <response code="200">Returns the list of all permissions.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PermissionDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PermissionDto>>> GetAllPermissions()
        {
            var query = new GetAllPermissionsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific permission by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the permission to retrieve.</param>
        /// <returns>The details of the specified permission.</returns>
        /// <response code="200">Returns the permission details.</response>
        /// <response code="403">If the user does not have the required 'Permissions.View' permission.</response>
        /// <response code="404">If a permission with the specified ID is not found.</response>
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
        /// Retrieves all permissions, grouped by their functional area (e.g., "Users", "Roles"). (Admin only)
        /// </summary>
        /// <returns>A list of permission groups, each containing a list of permissions.</returns>
        /// <response code="200">Returns the list of grouped permissions.</response>
        /// <response code="403">If the user does not have the required 'Permissions.View' permission.</response>
        /// <response code="500">If an internal server error occurs.</response>
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
        /// Retrieves all permissions assigned to a specific role. (Admin only)
        /// </summary>
        /// <param name="roleId">The ID of the role.</param>
        /// <returns>A list of permissions assigned to the specified role.</returns>
        /// <response code="200">Returns the list of permissions for the role.</response>
        /// <response code="403">If the user does not have the required 'Permissions.View' permission.</response>
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
        /// Retrieves all permissions assigned to a specific user, including those inherited from their role.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A comprehensive list of permissions for the user.</returns>
        /// <response code="200">Returns the list of all permissions for the user.</response>
        /// <response code="403">If the user does not have the required 'Permissions.View' permission.</response>
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
        /// Retrieves only the custom permissions (grant/revoke) specifically assigned to a user, excluding inherited role permissions.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of custom-assigned permissions for the user.</returns>
        /// <response code="200">Returns the list of custom permissions.</response>
        /// <response code="403">If the user does not have the required 'Permissions.View' permission.</response>
        [HttpGet("users/{userId}/custom-permissions")]
        [RequirePermission(PermissionNames.PermissionsView)]
        [ProducesResponseType(typeof(List<PermissionDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<PermissionDto>>> GetCustomPermissionsByUserId(int userId)
        {
            _logger.LogInformation("Getting custom permissions for user with ID: {UserId}", userId);
            var query = new GetUserCustomPermissionsQuery(userId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Updates the custom permissions for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose permissions are to be updated.</param>
        /// <param name="command">The command containing the permissions to grant or revoke.</param>
        /// <returns>A success response if the update is successful.</returns>
        /// <response code="204">If the user permissions were successfully updated.</response>
        /// <response code="400">If the request is invalid (e.g., validation errors).</response>
        /// <response code="404">If the specified user is not found.</response>
        /// <response code="500">If an internal server error occurs.</response>
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
        /// Checks if a specific user has a given permission.
        /// </summary>
        /// <param name="userId">The ID of the user to check.</param>
        /// <param name="permissionName">The name of the permission to check (e.g., "Users.View").</param>
        /// <returns>True if the user has the permission; otherwise, false.</returns>
        /// <response code="200">Returns the result of the permission check.</response>
        /// <response code="403">If the user does not have the required 'Permissions.Check' permission to perform the check.</response>
        /// <response code="404">If the specified user or permission name is not found.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpGet("user/{userId}/check/{permissionName}")]
        [RequirePermission(PermissionNames.PermissionsCheck)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> UserHasPermission(int userId, string permissionName)
        {
            var query = new GetUserHasPermissionQuery(userId, permissionName);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Manually adds missing page access permissions to the database. (Admin only)
        /// </summary>
        /// <remarks>
        /// This endpoint is typically used after initial setup or an update to ensure that core permissions
        /// defined in `PermissionNames` (e.g., `Pages.Revir.View`, `Pages.BilgiIslem.View`) exist in the database.
        /// Use with caution in a production environment.
        /// </remarks>
        /// <returns>A success message if the operation is successful.</returns>
        /// <response code="200">If the missing permissions were successfully checked and/or added.</response>
        /// <response code="401">If the user is not authenticated or is not an Admin.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpGet("add-missing-permissions")]
        [Authorize(Roles = RoleNames.Admin)]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddMissingPermissionsManually()
        {
            try
            {
                await _mediator.Send(new AddMissingPermissionsCommand());
                return Ok(new { message = "Eksik izinler başarıyla kontrol edildi ve/veya eklendi." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorAddingMissingPermissions);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(StatusCodes.Status500InternalServerError, string.Format(ErrorMessages.MissingPermissionsError, ex.Message)));
            }
        }

        /// <summary>
        /// Creates a new permission.
        /// </summary>
        /// <param name="command">The command for creating a permission.</param>
        /// <returns>The created permission.</returns>
        /// <response code="201">Returns the newly created permission.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="403">If the user does not have the required 'Permissions.Create' permission.</response>
        /// <response code="500">If an internal server error occurs.</response>
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
        /// Updates an existing permission.
        /// </summary>
        /// <param name="id">The ID of the permission to update.</param>
        /// <param name="command">The command for updating the permission.</param>
        /// <returns>An success response if the update is successful.</returns>
        /// <response code="204">If the permission was successfully updated.</response>
        /// <response code="400">If the request is invalid (e.g., ID mismatch).</response>
        /// <response code="403">If the user does not have the required 'Permissions.Update' permission.</response>
        /// <response code="404">If the permission is not found.</response>
        /// <response code="500">If an internal server error occurs.</response>
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
        /// Deletes a permission.
        /// </summary>
        /// <param name="id">The ID of the permission to delete.</param>
        /// <returns>A success response if the deletion is successful.</returns>
        /// <response code="204">If the permission was successfully deleted.</response>
        /// <response code="403">If the user does not have the required 'Permissions.Delete' permission.</response>
        /// <response code="404">If the permission is not found.</response>
        /// <response code="500">If an internal server error occurs.</response>
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

        /// <summary>
        /// Removes a specific custom permission from a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="permissionId">The ID of the permission to remove.</param>
        /// <returns>A success response if the operation is successful.</returns>
        /// <response code="204">If the permission was successfully removed from the user.</response>
        /// <response code="403">If the user does not have the required 'Permissions.Update' permission.</response>
        [HttpDelete("users/{userId}/permissions/{permissionId}")]
        [RequirePermission(PermissionNames.PermissionsUpdate)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RemoveUserPermission(int userId, int permissionId)
        {
            var command = new RemoveUserPermissionCommand { UserId = userId, PermissionId = permissionId };
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Resets all custom permissions for a user, so they only inherit permissions from their role.
        /// </summary>
        /// <param name="userId">The ID of the user whose permissions are to be reset.</param>
        /// <returns>A success response if the operation is successful.</returns>
        /// <response code="204">If the user's custom permissions were successfully reset.</response>
        /// <response code="403">If the user does not have the required 'Permissions.Update' permission.</response>
        [HttpPost("users/{userId}/reset-permissions")]
        [RequirePermission(PermissionNames.PermissionsUpdate)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ResetUserPermissions(int userId)
        {
            var command = new ResetUserPermissionsCommand(userId);
            await _mediator.Send(command);
            return NoContent();
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