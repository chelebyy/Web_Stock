using MediatR;
using Stock.Application.Common.Interfaces;
using Stock.Application.Features.Permissions.Queries.GetUserHasPermission;
using System.Threading.Tasks;

namespace Stock.Infrastructure.Identity
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IMediator _mediator;

        public AuthorizationService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> HasPermissionAsync(int userId, string permissionName)
        {
            var query = new GetUserHasPermissionQuery(userId, permissionName);
            return await _mediator.Send(query);
        }
    }
} 