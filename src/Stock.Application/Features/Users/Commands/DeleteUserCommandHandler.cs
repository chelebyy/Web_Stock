using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Exceptions;
using Stock.Application.Common.Interfaces;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications;
using Stock.Application.Features.Users.Commands.DeleteUser;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Users.Commands
{
    /// <summary>
    /// Handles the <see cref="DeleteUserCommand"/>.
    /// </summary>
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        private readonly ILogger<DeleteUserCommandHandler> _logger;
        private const string CacheKey = "users_all";

        public DeleteUserCommandHandler(
            IUnitOfWork unitOfWork,
            ICacheService cacheService,
            ILogger<DeleteUserCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            // 1. Silinecek kullanıcıyı bul
            var userSpec = new UserByIdSpecification(request.Id);
            var user = await _unitOfWork.Users.FirstOrDefaultAsync(userSpec, cancellationToken);

            if (user == null)
            {
                // Kullanıcı bulunamadıysa NotFoundException fırlatmak iyi bir pratik.
                throw new NotFoundException(nameof(User), request.Id);
            }

            // 2. Kullanıcıyı sil
            await _unitOfWork.Users.DeleteAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("User {UserId} deleted successfully.", request.Id);

            // Invalidate all paged user list caches
            await _cacheService.RemoveByPrefixAsync("users_page").ConfigureAwait(false);
            _logger.LogInformation("Cache invalidated for prefix: users_page");

            // Invalidate the specific user cache
            var specificCacheKey = $"users:{request.Id}";
            await _cacheService.RemoveAsync(specificCacheKey).ConfigureAwait(false);
            _logger.LogInformation("Cache invalidated for key: {SpecificCacheKey}", specificCacheKey);

            return Unit.Value;
        }
    }
} 