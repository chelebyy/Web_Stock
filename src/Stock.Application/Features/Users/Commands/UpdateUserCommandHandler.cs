using AutoMapper;
using MediatR;
// using Stock.Domain.Exceptions; // Kaldırıldı
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Stock.Domain.Specifications;
using Stock.Domain.Common; // Result ve DomainErrors için
// using Stock.Domain.Common.Result; // Kaldırıldı
// using Stock.Domain.Common.Errors; // Kaldırıldı
// using Stock.Domain.ValueObjects; // Kaldırıldı
using Stock.Application.Interfaces; // IUnitOfWork için
using Stock.Domain.Exceptions; // Eklendi
using Stock.Application.Constants;
using Stock.Application.Common.Interfaces;

namespace Stock.Application.Features.Users.Commands.UpdateUser;

/// <summary>
/// Handles the <see cref="UpdateUserCommand"/>.
/// </summary>
public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto?> // Dönüş tipi UserDto?
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILoggerManager<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher, ILoggerManager<UpdateUserCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<UserDto?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInfo(LogMessages.UpdatingUser, request.Id);

            var userSpec = new EntityByIdSpecification<User>(request.Id);
            var userToUpdate = await _unitOfWork.Users.FirstOrDefaultAsync(userSpec, cancellationToken);

            if (userToUpdate == null)
            {
                _logger.LogWarn(LogMessages.UserNotFoundById, request.Id);
                return null; // Veya NotFoundException fırlat
                // throw new NotFoundException(DomainErrors.User.NotFound(request.Id));
            }
            
            // Sicil kontrolü (değişmişse ve başkası tarafından kullanılıyorsa)
            if (userToUpdate.Sicil != request.Sicil)
            {
                var duplicateUserSpec = new UserBySicilSpecification(request.Sicil, false);
                var existingUser = await _unitOfWork.Users.FirstOrDefaultAsync(duplicateUserSpec, cancellationToken);
                if (existingUser != null && existingUser.Id != request.Id)
                {
                    _logger.LogWarn(LogMessages.DuplicateSicil, request.Sicil);
                    throw new DuplicateEntityException($"Bu sicil numarası zaten kullanılmaktadır: {request.Sicil}");
                }
                
                // Sicil güncellemesi için domain metodunu kullanalım
                var sicilResult = userToUpdate.UpdateSicil(request.Sicil);
                if (!sicilResult.IsSuccess)
                {
                    _logger.LogWarn(LogMessages.InvalidUserData, sicilResult.Error);
                    throw new ValidationException(sicilResult.Error);
                }
            }
            
            // Ad ve Soyad güncellemesi (domain metodunu kullanarak)
            var nameResult = userToUpdate.UpdateName(request.Adi, request.Soyadi);
            if (!nameResult.IsSuccess)
            {
                _logger.LogWarn(LogMessages.InvalidUserData, nameResult.Error);
                throw new ValidationException(nameResult.Error);
            }

            // Rol güncellemesi
            if (userToUpdate.RoleId != request.RoleId) 
            {
                if (request.RoleId.HasValue)
                {
                    var roleSpec = new EntityByIdSpecification<Role>(request.RoleId.Value);
                    var role = await _unitOfWork.Roles.FirstOrDefaultAsync(roleSpec, cancellationToken);
                    
                    if (role == null)
                    {
                        _logger.LogWarn(LogMessages.RoleNotFoundById, request.RoleId.Value);
                        throw new NotFoundException($"Rol bulunamadı: ID {request.RoleId.Value}");
                    }
                    
                    userToUpdate.AssignRole(request.RoleId.Value, role);
                }
                else 
                {
                    userToUpdate.RemoveRole();
                }
            }
            
            // Şifre güncellemesi
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                var newPasswordHash = _passwordHasher.HashPassword(request.Password);
                
                var passwordResult = userToUpdate.ChangePassword(newPasswordHash);
                if (!passwordResult.IsSuccess)
                {
                    throw new BadRequestException(passwordResult.Error);
                }
            }
            
            // IsAdmin güncellemesi
            if (userToUpdate.IsAdmin != request.IsAdmin)
            {
                userToUpdate.SetAdminStatus(request.IsAdmin);
            }

            _unitOfWork.Users.Update(userToUpdate);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInfo(LogMessages.UserUpdated, request.Id);

            // Güncellenmiş kullanıcıyı rolüyle getir
            var updatedUserSpec = new UsersWithRolesSpecification(request.Id);
            var updatedUserWithRole = await _unitOfWork.Users.FirstOrDefaultAsync(updatedUserSpec, cancellationToken);
            
            var userDto = _mapper.Map<UserDto>(updatedUserWithRole);
            return userDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LogMessages.ErrorUpdatingUser, request.Id);
            throw;
        }
    }
} 