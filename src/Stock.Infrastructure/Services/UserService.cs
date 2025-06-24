using Microsoft.EntityFrameworkCore;
using Stock.Application.Common.Interfaces;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stock.Application.Constants;
using AutoMapper;
using Stock.Domain.Specifications;
using Stock.Domain.Specifications.Users;

namespace Stock.Infrastructure.Services
{
    /// <summary>
    /// Kullanıcı yönetimi işlemlerini gerçekleştiren servis sınıfı.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerManager<UserService> _logger;
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// UserService sınıfının yapıcı metodu.
        /// </summary>
        /// <param name="unitOfWork">Veritabanı işlemleri için Unit of Work arayüzü.</param>
        /// <param name="mapper">DTO ve Entity arasındaki dönüşümleri gerçekleştiren IMapper arayüzü.</param>
        /// <param name="logger">Loglama işlemleri için ILoggerManager arayüzü.</param>
        /// <param name="currentUserService">Mevcut kullanıcı bilgilerine erişim için ICurrentUserService arayüzü.</param>
        public UserService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoggerManager<UserService> logger,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Tüm kullanıcıları getirir.
        /// </summary>
        /// <returns>Kullanıcı listesi.</returns>
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            try
            {
                _logger.LogInfo(LogMessages.GettingAllUsers);
                var spec = new AllSpecification<User>();
                var users = await _unitOfWork.Users.ListAsync(spec);
                _logger.LogInfo(LogMessages.UsersRetrieved, users.Count);
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorGettingUsers);
                throw;
            }
        }

        /// <summary>
        /// Belirtilen ID'ye sahip kullanıcıyı getirir.
        /// </summary>
        /// <param name="id">Kullanıcı ID'si.</param>
        /// <returns>Kullanıcı nesnesi veya null.</returns>
        public async Task<User?> GetUserByIdAsync(int id)
        {
            try
            {
                _logger.LogInfo(LogMessages.GettingUserById, id);
                var spec = new EntityByIdSpecification<User>(id);
                var user = await _unitOfWork.Users.FirstOrDefaultAsync(spec);
                
                if (user == null)
                {
                    _logger.LogWarn(LogMessages.UserNotFoundById, id);
                    return null;
                }

                _logger.LogInfo(LogMessages.UserRetrieved, id);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorGettingUserById, id);
                throw;
            }
        }

        /// <summary>
        /// Yeni bir kullanıcı oluşturur.
        /// </summary>
        /// <param name="user">Oluşturulacak kullanıcı nesnesi.</param>
        /// <returns>Oluşturulan kullanıcı nesnesi.</returns>
        public async Task<User> CreateUserAsync(User user)
        {
            try
            {
                _logger.LogInfo(LogMessages.CreatingUserWithSicil, user.Sicil);

                var existingUserSpecSicil = new UserBySicilSpecification(user.Sicil, includeRole: false);
                var existingUserBySicil = await _unitOfWork.Users.FirstOrDefaultAsync(existingUserSpecSicil);
                
                if (existingUserBySicil != null)
                {
                    _logger.LogWarn(LogMessages.DuplicateSicil, user.Sicil);
                    throw new InvalidOperationException(ErrorMessages.DuplicateSicil);
                }

                // DDD approach: Use factory method to create the entity
                var userResult = User.Create(
                    user.Adi,
                    user.Soyadi,
                    user.Sicil,
                    user.PasswordHash,
                    user.RoleId,
                    user.IsAdmin);

                if (!userResult.IsSuccess)
                {
                    _logger.LogWarn(LogMessages.InvalidUserData, userResult.Error);
                    throw new InvalidOperationException(userResult.Error);
                }

                var newUser = userResult.Value;
                newUser.CreatedAt = DateTime.UtcNow;
                
                await _unitOfWork.Users.AddAsync(newUser);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInfo(LogMessages.UserCreatedWithSicil, newUser.Id, newUser.Sicil);
                return newUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorCreatingUserWithSicil, user.Sicil);
                throw;
            }
        }

        /// <summary>
        /// Mevcut bir kullanıcıyı günceller.
        /// </summary>
        /// <param name="id">Güncellenecek kullanıcının ID'si.</param>
        /// <param name="user">Güncellenmiş kullanıcı bilgileri.</param>
        /// <returns>Güncellenen kullanıcı nesnesi.</returns>
        public async Task<User> UpdateUserAsync(int id, User user)
        {
            try
            {
                _logger.LogInfo(LogMessages.UpdatingUser, id);

                var existingUserSpec = new EntityByIdSpecification<User>(id);
                var existingUser = await _unitOfWork.Users.FirstOrDefaultAsync(existingUserSpec);
                if (existingUser == null)
                {
                    _logger.LogWarn(LogMessages.UserNotFoundById, id);
                    throw new InvalidOperationException(ErrorMessages.UserNotFound);
                }

                if (user.Sicil != existingUser.Sicil)
                {
                    var duplicateUserSpec = new UserBySicilSpecification(user.Sicil, includeRole: false);
                    var duplicateUser = await _unitOfWork.Users.FirstOrDefaultAsync(duplicateUserSpec);
                    
                    if (duplicateUser != null && duplicateUser.Id != id)
                    {
                        _logger.LogWarn(LogMessages.DuplicateSicil, user.Sicil);
                        throw new InvalidOperationException(ErrorMessages.DuplicateSicil);
                    }
                }

                // DDD approach: Use entity's behavior methods
                var sicilResult = existingUser.UpdateSicil(user.Sicil);
                if (!sicilResult.IsSuccess)
                {
                    throw new InvalidOperationException(sicilResult.Error);
                }

                var nameResult = existingUser.UpdateName(user.Adi, user.Soyadi);
                if (!nameResult.IsSuccess)
                {
                    throw new InvalidOperationException(nameResult.Error);
                }

                existingUser.SetAdminStatus(user.IsAdmin);

                // Handle role changes
                if (existingUser.RoleId != user.RoleId)
                {
                    if (user.RoleId.HasValue)
                    {
                        // Get the role entity if needed
                        var roleSpec = new EntityByIdSpecification<Role>(user.RoleId.Value);
                        var role = await _unitOfWork.Roles.FirstOrDefaultAsync(roleSpec);
                        existingUser.AssignRole(user.RoleId.Value, role);
                    }
                    else
                    {
                        existingUser.RemoveRole();
                    }
                }

                existingUser.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.Users.Update(existingUser);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInfo(LogMessages.UserUpdated, id);
                return existingUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorUpdatingUser, id);
                throw;
            }
        }

        /// <summary>
        /// Belirtilen ID'ye sahip kullanıcıyı siler.
        /// </summary>
        /// <param name="id">Silinecek kullanıcının ID'si.</param>
        /// <returns>İşlemin başarılı olup olmadığı.</returns>
        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                _logger.LogInfo(LogMessages.DeletingUser, id);

                var userSpec = new EntityByIdSpecification<User>(id);
                var user = await _unitOfWork.Users.FirstOrDefaultAsync(userSpec);
                if (user == null)
                {
                    _logger.LogWarn(LogMessages.UserNotFoundById, id);
                    return false;
                }

                if (user.IsAdmin)
                {
                    _logger.LogWarn(LogMessages.CannotDeleteAdminUser, id);
                    throw new InvalidOperationException(ErrorMessages.CannotDeleteAdminUser);
                }

                if (_currentUserService.UserId == id)
                {
                    _logger.LogWarn(LogMessages.CannotDeleteSelf, id);
                    throw new InvalidOperationException(ErrorMessages.CannotDeleteSelf);
                }

                _unitOfWork.Users.Delete(user);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInfo(LogMessages.UserDeleted, id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorDeletingUser, id);
                throw;
            }
        }

        /// <summary>
        /// Kullanıcının rolünü günceller.
        /// </summary>
        /// <param name="userId">Kullanıcı ID'si.</param>
        /// <param name="roleId">Yeni rol ID'si.</param>
        /// <returns>İşlemin başarılı olup olmadığı.</returns>
        public async Task<bool> UpdateUserRoleAsync(int userId, int roleId)
        {
            try
            {
                _logger.LogInfo(LogMessages.UpdatingUserRole, userId, roleId);

                var userSpec = new EntityByIdSpecification<User>(userId);
                var user = await _unitOfWork.Users.FirstOrDefaultAsync(userSpec);
                if (user == null)
                {
                    _logger.LogWarn(LogMessages.UserNotFoundById, userId);
                    return false;
                }

                var roleSpec = new EntityByIdSpecification<Role>(roleId);
                var role = await _unitOfWork.Roles.FirstOrDefaultAsync(roleSpec);
                if (role == null)
                {
                    _logger.LogWarn(LogMessages.RoleNotFoundById, roleId);
                    return false;
                }

                // DDD yaklaşımı: User entity'sinin metodu ile rol atama
                user.AssignRole(roleId, role);
                user.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInfo(LogMessages.UserRoleUpdated, userId, roleId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorUpdatingUserRole, userId, roleId);
                throw;
            }
        }
    }
} 