using MediatR;

namespace Stock.Application.Features.Roles.Commands.DeleteRole;

/// <summary>
/// Represents the command to delete a role.
/// </summary>
public class DeleteRoleCommand : IRequest<bool>
{
    /// <summary>
    /// Gets or sets the ID of the role to delete.
    /// </summary>
    public int Id { get; set; }

    public DeleteRoleCommand(int id)
    {
        Id = id;
    }
} 