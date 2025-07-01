using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Constants;
using Stock.Application.Features.Users.Queries.GetAllUsers;
using Stock.Application.Features.Users.Queries.GetUserById;
using Stock.Application.Features.Users.Commands;
using Stock.Application.Features.Users.Commands.DeleteUser;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using Stock.Application.Models.DTOs;
using System.Collections.Generic;
using FluentValidation;
using Stock.Domain.Exceptions;
using Stock.Application.Models;


namespace Stock.API.Controllers
{
    /// <summary>
    /// Manages user-related operations such as retrieving, creating, updating, and deleting users.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator for sending commands and queries.</param>
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves a paginated list of all users. (Admin only)
        /// </summary>
        /// <param name="query">The query parameters for pagination and filtering.</param>
        /// <returns>A paginated list of users.</returns>
        /// <response code="200">Returns the paginated list of users.</response>
        /// <response code="401">Unauthorized if the user is not authenticated.</response>
        /// <response code="403">Forbidden if the user is not an Admin.</response>
        [HttpGet]
        [Authorize(Roles = RoleNames.Admin)]
        [ProducesResponseType(typeof(PagedResponse<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllUsersQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific user by their unique ID.
        /// </summary>
        /// <remarks>
        /// An authenticated user can retrieve their own details. An Admin can retrieve any user's details.
        /// </remarks>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The user's details.</returns>
        /// <response code="200">Returns the user's details.</response>
        /// <response code="401">Unauthorized if the user is not authenticated.</response>
        /// <response code="403">Forbidden if the user is trying to access another user's details without Admin privileges.</response>
        /// <response code="404">If a user with the specified ID is not found.</response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery { Id = id });
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        /// <summary>
        /// Creates a new user. (Admin only)
        /// </summary>
        /// <param name="command">The command containing the details for the new user.</param>
        /// <returns>The newly created user's details.</returns>
        /// <response code="201">Returns the newly created user.</response>
        /// <response code="400">If the request is invalid (e.g., validation errors).</response>
        /// <response code="401">Unauthorized if the user is not authenticated.</response>
        /// <response code="403">Forbidden if the user is not an Admin.</response>
        /// <response code="409">If a user with the same registration number (sicil) already exists.</response>
        [HttpPost]
        [Authorize(Roles = RoleNames.Admin)]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
            }
            // Handle error cases, e.g., conflict or bad request
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Updates an existing user. (Admin only)
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="command">The command containing the updated details for the user.</param>
        /// <returns>The updated user's details.</returns>
        /// <response code="200">Returns the updated user.</response>
        /// <response code="400">If the request is invalid (e.g., ID mismatch, validation errors).</response>
        /// <response code="401">Unauthorized if the user is not authenticated.</response>
        /// <response code="403">Forbidden if the user is not an Admin.</response>
        /// <response code="404">If a user with the specified ID is not found.</response>
        /// <response code="409">If a conflict occurs (e.g., unique constraint violation).</response>
        [HttpPut("{id}")]
        [Authorize(Roles = RoleNames.Admin)]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserCommand command)
        {
            if (id != command.Id)
            {
                throw new BadRequestException(ErrorMessages.RouteBodyIdMismatch);
            }
            
            await _mediator.Send(command);
            
            return NoContent(); // Return 204 No Content on successful update
        }

        /// <summary>
        /// Deletes a user by their unique ID. (Admin only)
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>A success response if the user is deleted.</returns>
        /// <response code="204">If the user was successfully deleted.</response>
        /// <response code="401">Unauthorized if the user is not authenticated.</response>
        /// <response code="403">Forbidden if the user is not an Admin.</response>
        /// <response code="404">If a user with the specified ID is not found.</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleNames.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteUserCommand { Id = id });
            return NoContent();
        }
    }
} 