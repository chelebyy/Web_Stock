using MediatR;

namespace Stock.Application.Features.Permissions.Queries.GetUserHasPermission
{
    public class GetUserHasPermissionQuery : IRequest<bool>
    {
        public int UserId { get; set; }
        public string PermissionName { get; set; }

        public GetUserHasPermissionQuery(int userId, string permissionName)
        {
            UserId = userId;
            PermissionName = permissionName;
        }
    }
} 