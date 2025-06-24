using Microsoft.EntityFrameworkCore;
using Stock.Application.Common.Interfaces;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stock.Application.Constants;
using Stock.Domain.Specifications;
using Stock.Domain.Entities;
using Stock.Domain.Specifications.RolePermissions;
using Stock.Domain.Specifications.Users;
using Stock.Domain.Specifications.UserPermissions;
using AutoMapper;

namespace Stock.Infrastructure.Services
{
    /// <summary>
    /// İzin yönetimi ile ilgili işlemleri gerçekleştiren servis.
    /// IPermissionService arayüzünü implemente eder.
    /// </summary>
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly ILoggerManager<PermissionService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// PermissionService sınıfının yapıcı metodu.
        /// Gerekli bağımlılıkları (IPermissionRepository, ILoggerManager, IUnitOfWork) enjekte eder.
        /// </summary>
        /// <param name="permissionRepository">İzin repository arayüzü.</param>
        /// <param name="logger">Loglama işlemleri için ILoggerManager arayüzü.</param>
        /// <param name="unitOfWork">Unit of Work arayüzü.</param>
        /// <param name="mapper">Mapper arayüzü.</param>
        public PermissionService(
            IPermissionRepository permissionRepository,
            ILoggerManager<PermissionService> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Sistemdeki tüm izinleri asenkron olarak getirir.
        /// </summary>
        /// <returns>Tüm izinlerin listesi (PermissionDto).</returns>
        public async Task<List<PermissionDto>> GetAllPermissionsAsync()
        {
            try
            {
                var spec = new AllSpecification<Permission>();
                var permissions = await _permissionRepository.ListAsync(spec);
                return permissions.Select(p => MapToDto(p)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorGettingAllPermissions);
                return new List<PermissionDto>();
            }
        }

        /// <summary>
        /// Sistemdeki tüm izinleri gruplarına göre asenkron olarak getirir.
        /// </summary>
        /// <returns>Gruplanmış izinlerin listesi (PermissionGroupDto).</returns>
        public async Task<List<PermissionGroupDto>> GetPermissionsByGroupsAsync()
        {
            try
            {
                var spec = new AllSpecification<Permission>();
                var permissions = await _permissionRepository.ListAsync(spec);

                var groupedPermissions = permissions
                    .GroupBy(p => p.Group ?? PermissionGroupNames.Other)
                    .Select(g => new PermissionGroupDto
                    {
                        Group = g.Key,
                        Permissions = g.Select(p => MapToDto(p)).OrderBy(p => p.Name).ToList()
                    })
                    .OrderBy(g => g.Group)
                    .ToList();

                return groupedPermissions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorGettingPermissionsByGroup);
                return new List<PermissionGroupDto>();
            }
        }

        /// <summary>
        /// Belirtilen role atanmış izinleri asenkron olarak getirir.
        /// </summary>
        /// <param name="roleId">İzinleri getirilecek rolün ID'si.</param>
        /// <returns>Role ait izinlerin listesi (PermissionDto).</returns>
        public async Task<List<PermissionDto>> GetPermissionsByRoleIdAsync(int roleId)
        {
            try
            {
                var rolePermissionRepo = _unitOfWork.GetRepository<RolePermission>();
                var spec = new PermissionsByRoleIdSpecification(roleId);
                var rolePermissions = await rolePermissionRepo.ListAsync(spec);

                return rolePermissions
                    .Where(rp => rp.Permission != null)
                    .Select(rp => MapToDto(rp.Permission))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorGettingPermissionsByRole, roleId);
                return new List<PermissionDto>();
            }
        }

        /// <summary>
        /// Belirtilen kullanıcıya atanmış (doğrudan veya rolünden gelen) izinleri asenkron olarak getirir.
        /// Bu metodun karmaşıklığı nedeniyle, doğrudan UnitOfWork üzerinden repository'lere erişim tercih edildi.
        /// İdeal senaryoda, bu mantık Application katmanında bir QueryHandler'a taşınabilir veya
        /// daha sofistike Specification'lar (örn. Aggregate Specification) kullanılabilir.
        /// </summary>
        /// <param name="userId">İzinleri getirilecek kullanıcının ID'si.</param>
        /// <returns>Kullanıcıya ait izinlerin listesi (PermissionDto).</returns>
        public async Task<List<PermissionDto>> GetPermissionsByUserIdAsync(int userId)
        {
            try
            {
                var userRepo = _unitOfWork.Users;
                var userPermissionRepo = _unitOfWork.GetRepository<UserPermission>();

                var userSpec = new UserWithRoleAndPermissionsSpecification(userId);
                var user = await userRepo.FirstOrDefaultAsync(userSpec);

                if (user == null)
                {
                    _logger.LogWarn(LogMessages.UserNotFoundById, userId);
                    return new List<PermissionDto>();
                }

                var allPermissions = new Dictionary<int, PermissionDto>();

                if (user.Role?.RolePermissions != null)
                {
                    foreach (var rp in user.Role.RolePermissions)
                    {
                        if (rp.Permission != null && !allPermissions.ContainsKey(rp.Permission.Id))
                        {
                            allPermissions.Add(rp.Permission.Id, MapToDto(rp.Permission, true, user.Role.Name));
                        }
                    }
                }

                var userPermissionsSpec = new UserPermissionsWithDetailsSpecification(userId);
                var userPermissions = await userPermissionRepo.ListAsync(userPermissionsSpec);

                foreach (var up in userPermissions)
                {
                    if (up.Permission != null)
                    {
                        if (allPermissions.TryGetValue(up.Permission.Id, out var existingDto))
                        {
                            if (!up.IsGranted)
                            {
                                existingDto.IsGranted = false;
                                existingDto.IsCustom = true;
                            }
                            else if (existingDto.IsFromRole)
                            {
                                existingDto.IsCustom = true;
                            }
                        }
                        else if (up.IsGranted)
                        {
                            var newDto = MapToDto(up.Permission, false);
                            newDto.IsCustom = true;
                            allPermissions.Add(up.Permission.Id, newDto);
                        }
                    }
                }

                return allPermissions.Values.OrderBy(p => p.Group).ThenBy(p => p.Name).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorGettingPermissionsByUser, userId);
                return new List<PermissionDto>();
            }
        }

        /// <summary>
        /// Belirtilen role toplu olarak izin atar (önceki izinleri siler, yenilerini ekler).
        /// </summary>
        /// <param name="roleId">İzin atanacak rolün ID'si.</param>
        /// <param name="permissionIds">Atanacak izinlerin ID listesi.</param>
        /// <returns>İşlemin başarı durumunu belirten Task&lt;bool&gt;.</returns>
        public async Task<bool> AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds)
        {
            try
            {
                _logger.LogInfo(LogMessages.AssigningPermissionsToRole, roleId, permissionIds?.Count ?? 0);

                var rolePermissionRepo = _unitOfWork.GetRepository<RolePermission>();

                var existingPermissionsSpec = new RolePermissionsByRoleIdSpecification(roleId);
                var existingPermissions = await rolePermissionRepo.ListAsync(existingPermissionsSpec);
                
                if (existingPermissions.Any())
                {
                    rolePermissionRepo.DeleteRange(existingPermissions);
                    _logger.LogDebug("{Count} adet mevcut izin ilişkisi silinmek üzere işaretlendi: RoleId {RoleId}", existingPermissions.Count, roleId);
                }

                if (permissionIds != null && permissionIds.Count > 0)
                {
                    var newPermissions = permissionIds.Select(pid => 
                        RolePermission.Create(roleId, pid)).ToList();
                    
                    await rolePermissionRepo.AddRangeAsync(newPermissions);
                    _logger.LogDebug("{Count} adet yeni izin ilişkisi ekleniyor: RoleId {RoleId}", newPermissions.Count, roleId);
                }

                await _unitOfWork.SaveChangesAsync();
                _logger.LogInfo(LogMessages.PermissionsAssignedToRole, roleId, permissionIds?.Count ?? 0);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorAssigningPermissionsToRole, roleId);
                return false;
            }
        }

        /// <summary>
        /// Belirtilen kullanıcıya tek bir izin atar veya mevcut iznin durumunu günceller.
        /// </summary>
        /// <param name="userId">İzin atanacak kullanıcının ID'si.</param>
        /// <param name="permissionId">Atanacak veya güncellenecek iznin ID'si.</param>
        /// <param name="isGranted">İzin veriliyorsa true, kaldırılıyorsa false.</param>
        /// <returns>İşlemin başarı durumunu belirten Task&lt;bool&gt;.</returns>
        public async Task<bool> AssignPermissionToUserAsync(int userId, int permissionId, bool isGranted)
        {
            try
            {
                _logger.LogInfo(LogMessages.AssigningPermissionToUser, userId, permissionId, isGranted);
                var userPermissionRepo = _unitOfWork.GetRepository<UserPermission>();
                
                var spec = new UserPermissionSpecification(userId, permissionId);
                var existingPermission = await userPermissionRepo.FirstOrDefaultAsync(spec);

                if (existingPermission != null)
                {
                    if (existingPermission.IsGranted != isGranted)
                    {
                        existingPermission.UpdateGrantStatus(isGranted);
                        userPermissionRepo.Update(existingPermission);
                        _logger.LogInfo(LogMessages.UserPermissionUpdated, isGranted, userId, permissionId);
                    }
                }
                else
                {
                    var newUserPermission = UserPermission.Create(userId, permissionId, isGranted);
                    await userPermissionRepo.AddAsync(newUserPermission);
                    _logger.LogInfo(LogMessages.UserPermissionAdded, isGranted, userId, permissionId);
                }
                
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInfo(LogMessages.PermissionAssignedToUser, userId, permissionId, isGranted);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorAssigningUserPermission, userId, permissionId);
                return false;
            }
        }

        /// <summary>
        /// Belirtilen kullanıcıdan belirtilen izni (özel olarak atanmışsa) kaldırır.
        /// </summary>
        /// <param name="userId">İzni kaldırılacak kullanıcının ID'si.</param>
        /// <param name="permissionId">Kaldırılacak iznin ID'si.</param>
        /// <returns>İşlemin başarı durumunu belirten Task&lt;bool&gt;.</returns>
        public async Task<bool> RemoveUserPermissionAsync(int userId, int permissionId)
        {
            try
            {
                _logger.LogInfo(LogMessages.RemovingPermissionFromUser, userId, permissionId);
                var userPermissionRepo = _unitOfWork.GetRepository<UserPermission>();
                var spec = new UserPermissionSpecification(userId, permissionId);
                var userPermission = await userPermissionRepo.FirstOrDefaultAsync(spec);

                if (userPermission != null)
                {
                    userPermissionRepo.Delete(userPermission);
                    await _unitOfWork.SaveChangesAsync();
                    _logger.LogInfo(LogMessages.PermissionRemovedFromUser, userId, permissionId);
                    return true;
                }
                
                _logger.LogWarn(LogMessages.UserPermissionNotFoundForRemoval, userId, permissionId);
                return true; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorRemovingUserPermission, userId, permissionId);
                return false;
            }
        }

        /// <summary>
        /// Belirtilen kullanıcının tüm özel izinlerini kaldırır ve rolünün varsayılan izinlerine döndürür.
        /// </summary>
        /// <param name="userId">İzinleri sıfırlanacak kullanıcının ID'si.</param>
        /// <returns>İşlemin başarı durumunu belirten Task&lt;bool&gt;.</returns>
        public async Task<bool> ResetUserToRolePermissionsAsync(int userId)
        {
            try
            {
                _logger.LogInfo(LogMessages.ResettingPermissionsForUser, userId);
                var userPermissionRepo = _unitOfWork.GetRepository<UserPermission>();
                var spec = new UserPermissionsByUserIdSpecification(userId);
                var userPermissions = await userPermissionRepo.ListAsync(spec);

                if (userPermissions.Any())
                {
                    userPermissionRepo.DeleteRange(userPermissions);
                    await _unitOfWork.SaveChangesAsync();
                    _logger.LogInfo(LogMessages.PermissionsResetForUser, userId);
                    return true;
                }

                _logger.LogInfo(LogMessages.UserPermissionsResetNoAction, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorResettingUserPermissions, userId);
                return false;
            }
        }

        /// <summary>
        /// Belirtilen kullanıcının belirtilen izne sahip olup olmadığını asenkron olarak kontrol eder.
        /// İzin kontrolü hem doğrudan kullanıcıya atanmış izinleri hem de kullanıcının rolünden gelen izinleri dikkate alır.
        /// Doğrudan kullanıcıya atanmış ve 'IsGranted = false' olan izinler, rolden gelse bile geçersiz kılar.
        /// </summary>
        /// <param name="userId">Kontrol edilecek kullanıcının ID'si.</param>
        /// <param name="permissionName">Kontrol edilecek iznin adı.</param>
        /// <returns>Kullanıcının izne sahip olup olmadığını belirten Task&lt;bool&gt;.</returns>
        public async Task<bool> UserHasPermissionAsync(int userId, string permissionName)
        {
            try
            {
                var userRepo = _unitOfWork.Users;
                var userPermissionRepo = _unitOfWork.GetRepository<UserPermission>();

                var userSpec = new EntityByIdSpecification<User>(userId, u => u.Role);
                var user = await userRepo.FirstOrDefaultAsync(userSpec);

                if (user == null)
                {
                    _logger.LogDebug("Kullanıcı {UserId} bulunamadı veya rolü/rol izinleri yok.", userId);
                    return false;
                }

                // Check role permissions only if user has a role and the role has permissions
                var hasPermissionFromRole = user.Role != null && user.Role.RolePermissions != null && 
                                          user.Role.RolePermissions.Any(rp => rp.Permission?.Name == permissionName);

                if (hasPermissionFromRole)
                {
                    _logger.LogDebug("Kullanıcı {UserId}, '{PermissionName}' iznine sahip (rolden geldi).", userId, permissionName);
                    return true;
                }

                _logger.LogDebug("Kullanıcı {UserId}, '{PermissionName}' iznine sahip değil (ne doğrudan ne de rolden).", userId, permissionName);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorCheckingUserPermission, userId, permissionName);
                return false;
            }
        }

        /// <summary>
        /// Permission entity nesnesini PermissionDto nesnesine dönüştürür.
        /// </summary>
        /// <param name="permission">Dönüştürülecek Permission nesnesi.</param>
        /// <param name="isFromRole">İzin rolden mi geliyor?</param>
        /// <param name="roleName">Eğer izin rolden geliyorsa rolün adı.</param>
        /// <returns>Dönüştürülmüş PermissionDto nesnesi.</returns>
        private PermissionDto MapToDto(Permission permission, bool isFromRole = false, string? roleName = null)
        {
            return new PermissionDto
            {
                Id = permission.Id,
                Name = permission.Name,
                Description = permission.Description,
                ResourceType = permission.ResourceType ?? "",
                Group = permission.Group,
                IsFromRole = isFromRole,
                IsCustom = !isFromRole,
                RoleName = isFromRole ? roleName : null,
                IsGranted = true
            };
        }
    }
} 