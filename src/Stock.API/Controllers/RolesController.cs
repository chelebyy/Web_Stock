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
    [Route("api/roles")]
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
        /// <param name="logger">ILogger nesnesi.</param>
        public RolesController(IMediator mediator, ILogger<RolesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a paginated list of all roles.
        /// </summary>
        /// <param name="query">The query parameters for pagination, sorting, and filtering.</param>
        /// <returns>A paginated list of roles, including details like name, description, and user count.</returns>
        /// <response code="200">Returns the paginated list of roles.</response>
        /// <response code="500">If an internal server error occurs while retrieving the roles.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<RoleSlimDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PagedResponse<RoleSlimDto>>> GetAllRoles([FromQuery] GetAllRolesQuery query)
        {
            _logger.LogInformation("Fetching all roles.");
            var roles = await _mediator.Send(query);
            return Ok(roles);
        }

        /// <summary>
        /// Retrieves the details of a specific role by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the role to retrieve.</param>
        /// <returns>The detailed information for the role, including the list of users in it.</returns>
        /// <response code="200">Returns the role details.</response>
        /// <response code="404">If a role with the specified ID is not found.</response>
        /// <response code="500">If an internal server error occurs.</response>
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
        /// Creates a new role.
        /// </summary>
        /// <param name="command">The command containing the details for the new role.</param>
        /// <returns>The newly created role's details.</returns>
        /// <response code="201">Returns the newly created role.</response>
        /// <response code="400">If the request is invalid (e.g., validation errors).</response>
        /// <response code="409">If a role with the same name already exists.</response>
        /// <response code="500">If an internal server error occurs.</response>
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
                return BadRequest(new ValidationErrorResponse(StatusCodes.Status400BadRequest, ex.Message, errors));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error creating role: {command.Name}");
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse(StatusCodes.Status500InternalServerError, "An unexpected error occurred."));
            }
        }

        /// <summary>
        /// Updates an existing role.
        /// </summary>
        /// <param name="id">The ID of the role to update.</param>
        /// <param name="command">The command containing the updated role details.</param>
        /// <returns>A success response if the update is successful.</returns>
        /// <response code="204">If the role was successfully updated.</response>
        /// <response code="400">If the request is invalid (e.g., ID mismatch or validation errors).</response>
        /// <response code="404">If a role with the specified ID is not found.</response>
        /// <response code="409">If a role with the same name already exists.</response>
        /// <response code="500">If an internal server error occurs.</response>
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
                return BadRequest(new ErrorResponse(StatusCodes.Status400BadRequest, "ID mismatch between route and body."));
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
                return BadRequest(new ValidationErrorResponse(StatusCodes.Status400BadRequest, ex.Message, errors));
            }
            catch (Stock.Domain.Exceptions.NotFoundException ex)
            {
                _logger.LogWarning(ex, $"Role with id: {id} not found for update.");
                return NotFound(new ErrorResponse(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error updating role: {id}");
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse(StatusCodes.Status500InternalServerError, "An unexpected error occurred."));
            }
        }

        /// <summary>
        /// Updates the permissions for a specific role. This replaces all existing permissions with the new set provided.
        /// </summary>
        /// <param name="id">The ID of the role whose permissions are to be updated.</param>
        /// <param name="command">The command containing the new list of permission IDs.</param>
        /// <returns>A success response if the update is successful.</returns>
        /// <response code="204">If the role permissions were successfully updated.</response>
        /// <response code="400">If the request is invalid (e.g., mismatched IDs, validation errors).</response>
        /// <response code="404">If a role with the specified ID is not found.</response>
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
                return BadRequest(new ValidationErrorResponse(StatusCodes.Status400BadRequest, ex.Message, errors));
            }
            catch (Stock.Domain.Exceptions.NotFoundException ex)
            {
                _logger.LogWarning(ex, $"Role with id: {id} not found for permission update.");
                return NotFound(new ErrorResponse(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error updating permissions for role: {id}");
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse(StatusCodes.Status500InternalServerError, "An unexpected error occurred."));
            }
        }

        /// <summary>
        /// Deletes a specific role by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the role to delete.</param>
        /// <returns>A success response if the deletion is successful.</returns>
        /// <response code="204">If the role was successfully deleted.</response>
        /// <response code="400">If the role cannot be deleted (e.g., it is currently in use by users).</response>
        /// <response code="404">If a role with the specified ID is not found.</response>
        /// <response code="500">If an internal server error occurs.</response>
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
                return NotFound(new ErrorResponse(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (Stock.Domain.Exceptions.ConflictException ex)
            {
                _logger.LogWarning(ex, $"Attempted to delete role with id: {id} but it is in use.");
                return BadRequest(new ErrorResponse(StatusCodes.Status400BadRequest, ex.Message));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error deleting role: {id}");
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse(StatusCodes.Status500InternalServerError, "An unexpected error occurred."));
            }
        }
    }
} 