using MediatR;
using Stock.Application.Models.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Stock.Application.Features.Roles.Commands.CreateRole
{
    /// <summary>
    /// Represents the command to create a new role.
    /// </summary>
    public class CreateRoleCommand : IRequest<RoleDto>
    {
        /// <summary>
        /// Gets or sets the name of the new role.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Rol açıklaması.
        /// </summary>
        public string? Description { get; set; }
    }
} 