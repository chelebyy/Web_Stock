using MediatR;
using Stock.Application.Models.DTOs;

namespace Stock.Application.Features.Roles.Commands.UpdateRole
{
    /// <summary>
    /// Represents the command to update an existing role.
    /// </summary>
    public class UpdateRoleCommand : IRequest<Unit>
    {
        /// <summary>
        /// Gets or sets the ID of the role to update.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the new name for the role.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Yeni rol açıklaması.
        /// </summary>
        public string? Description { get; set; }
    }
} 