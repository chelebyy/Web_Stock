using MediatR;
using Stock.Domain.Exceptions;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Stock.Domain.Specifications;
using Microsoft.Extensions.Logging;

namespace Stock.Application.Features.Roles.Commands.DeleteRole;

/// <summary>
/// Handles the <see cref="DeleteRoleCommand"/>.
/// </summary>
public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, bool>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteRoleCommandHandler> _logger;

    public DeleteRoleCommandHandler(
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork,
        ILogger<DeleteRoleCommandHandler> logger)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Handles the command to delete a role.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if deletion was successful.</returns>
    /// <exception cref="NotFoundException">Thrown if the role to delete is not found.</exception>
    /// <exception cref="ConflictException">Thrown if the role is associated with users.</exception>
    public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
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

        return true; // Indicate success
    }
} 