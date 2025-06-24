using MediatR;
using Stock.Application.Models.DTOs;

namespace Stock.Application.Features.Permissions.Queries.GetPermissionById;

public class GetPermissionByIdQuery : IRequest<PermissionDto>
{
    public int Id { get; set; }

    public GetPermissionByIdQuery(int id)
    {
        Id = id;
    }
} 