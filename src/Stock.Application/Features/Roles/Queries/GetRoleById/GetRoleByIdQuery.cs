using MediatR;
using Stock.Application.Models.DTOs;

namespace Stock.Application.Features.Roles.Queries.GetRoleById
{
    /// <summary>
    /// Represents the query to retrieve a specific role by its ID.
    /// </summary>
    public class GetRoleByIdQuery : IRequest<RoleDto>
    {
        /// <summary>
        /// Gets or sets the ID of the role to retrieve.
        /// </summary>
        public int Id { get; set; }

        public GetRoleByIdQuery(int id)
        {
            Id = id;
        }
    }
} 