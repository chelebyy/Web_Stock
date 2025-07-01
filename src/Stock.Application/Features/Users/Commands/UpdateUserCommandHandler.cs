using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Constants;
using Stock.Application.Common.Exceptions;
using Stock.Application.Common.Interfaces;
using Stock.Domain.Common;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications;
using Stock.Domain.ValueObjects;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Users.Commands
{
    /// <summary>
    /// Handles the <see cref="UpdateUserCommand"/>.
    /// </summary>
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly ILogger<UpdateUserCommandHandler> _logger;
        private const string CacheKey = "users_all";

        public UpdateUserCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICacheService cacheService,
            ILogger<UpdateUserCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var userSpec = new UserByIdSpecification(request.Id);
            var user = await _unitOfWork.Users.FirstOrDefaultAsync(userSpec, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), request.Id);
            }

            var sicilResult = Sicil.Create(request.Sicil);
            var fullNameResult = FullName.Create(request.Adi, request.Soyadi);

            if (sicilResult.IsFailure)
            {
                throw new DomainException(sicilResult.Error);
            }

            if (fullNameResult.IsFailure)
            {
                throw new DomainException(fullNameResult.Error);
            }

            var updateSicilResult = user.UpdateSicil(sicilResult.Value);
            if (updateSicilResult.IsFailure)
            {
                throw new DomainException(updateSicilResult.Error);
            }

            var updateNameResult = user.UpdateName(fullNameResult.Value);
            if (updateNameResult.IsFailure)
            {
                throw new DomainException(updateNameResult.Error);
            }

            user.AssignRole(request.RoleId, null); // Role navigation will be loaded separately if needed

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User {UserId} updated successfully.", user.Id);

            // Invalidate all paged user list caches
            await _cacheService.RemoveByPrefixAsync("users_page").ConfigureAwait(false);
            _logger.LogInformation("Cache invalidated for prefix: users_page");

            // Invalidate the specific user cache
            var specificCacheKey = $"users:{request.Id}";
            await _cacheService.RemoveAsync(specificCacheKey).ConfigureAwait(false);
            _logger.LogInformation("Cache invalidated for key: {SpecificCacheKey}", specificCacheKey);

            // Invalidate the user permissions cache
            var permissionsCacheKey = $"user-permissions:{request.Id}";
            await _cacheService.RemoveAsync(permissionsCacheKey).ConfigureAwait(false);
            _logger.LogInformation("Cache invalidated for key: {PermissionsCacheKey}", permissionsCacheKey);

            return Unit.Value;
        }
    }
} 