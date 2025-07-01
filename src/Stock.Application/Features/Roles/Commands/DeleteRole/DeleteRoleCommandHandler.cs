using MediatR;
using Stock.Domain.Exceptions;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Stock.Domain.Specifications;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Interfaces;

namespace Stock.Application.Features.Roles.Commands.DeleteRole;

/// <summary>
/// Handles the <see cref="DeleteRoleCommand"/>.
/// </summary>
public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Unit>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteRoleCommandHandler> _logger;
    private readonly ICacheService _cacheService;

    public DeleteRoleCommandHandler(
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork,
        ILogger<DeleteRoleCommandHandler> logger,
        ICacheService cacheService)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _cacheService = cacheService;
    }

    /// <summary>
    /// Handles the command to delete a role.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if deletion was successful.</returns>
    /// <exception cref="NotFoundException">Thrown if the role to delete is not found.</exception>
    /// <exception cref="ConflictException">Thrown if the role is associated with users.</exception>
    public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{RoleId} ID'li rol siliniyor.", request.Id);
        var spec = new RoleByIdSpecification(request.Id);
        var roleToDelete = await _roleRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (roleToDelete == null)
        {
            _logger.LogWarning("{RoleId} ID'li rol bulunamadı.", request.Id);
            throw new NotFoundException(nameof(Role), request.Id);
        }

        // Rolün kullanıcılara atanıp atanmadığını kontrol et (varsa)
        // var usersSpec = new UsersByRoleIdSpecification(request.Id);
        // var userCount = await _unitOfWork.Users.CountAsync(usersSpec, cancellationToken);
        // if (userCount > 0)
        // {
        //     throw new ConflictException("Bu rol kullanıcılara atanmış ve silinemez.");
        // }

        _roleRepository.Delete(roleToDelete);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("{RoleId} ID'li rol başarıyla silindi.", request.Id);

        // Invalidate both the list cache and the specific item cache
        var listCachePrefix = "roles_page";
        await _cacheService.RemoveByPrefixAsync(listCachePrefix).ConfigureAwait(false);
        _logger.LogInformation("Cache invalidated for prefix: {CachePrefix}", listCachePrefix);

        var specificCacheKey = $"roles:{request.Id}";
        await _cacheService.RemoveAsync(specificCacheKey).ConfigureAwait(false);
        _logger.LogInformation("Cache invalidated for key: {SpecificCacheKey}", specificCacheKey);

        return Unit.Value;
    }
} 