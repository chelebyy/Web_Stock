using Stock.Application.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stock.Application.Common.Interfaces
{
    /// <summary>
    /// İzin yönetimi işlemleri için arayüz.
    /// İzinleri listeleme, gruplama, role veya kullanıcıya atama, kaldırma ve kullanıcının belirli bir izne sahip olup olmadığını kontrol etme operasyonlarını tanımlar.
    /// </summary>
    public interface IPermissionService
    {
        /// <summary>
        /// Sistemdeki tüm izinleri asenkron olarak getirir.
        /// </summary>
        /// <returns>Tüm izinlerin listesini (<see cref="PermissionDto"/>) içeren bir Task.</returns>
        Task<List<PermissionDto>> GetAllPermissionsAsync();

        /// <summary>
        /// Sistemdeki tüm izinleri gruplarına göre asenkron olarak getirir.
        /// </summary>
        /// <returns>Gruplanmış izinlerin listesini (<see cref="PermissionGroupDto"/>) içeren bir Task.</returns>
        Task<List<PermissionGroupDto>> GetPermissionsByGroupsAsync();

        /// <summary>
        /// Belirtilen role atanmış tüm izinleri (doğrudan veya miras yoluyla) asenkron olarak getirir.
        /// </summary>
        /// <param name="roleId">İzinleri getirilecek rolün ID'si.</param>
        /// <returns>Role ait izinlerin listesini (<see cref="PermissionDto"/>) içeren bir Task.</returns>
        Task<List<PermissionDto>> GetPermissionsByRoleIdAsync(int roleId);

        /// <summary>
        /// Belirtilen kullanıcıya atanmış tüm izinleri (doğrudan veya rolünden gelen) asenkron olarak getirir.
        /// </summary>
        /// <param name="userId">İzinleri getirilecek kullanıcının ID'si.</param>
        /// <returns>Kullanıcıya ait izinlerin listesini (<see cref="PermissionDto"/>) içeren bir Task.</returns>
        Task<List<PermissionDto>> GetPermissionsByUserIdAsync(int userId);

        /// <summary>
        /// Belirtilen role bir veya daha fazla izni asenkron olarak atar.
        /// Bu işlem genellikle rolün mevcut izinlerini temizleyip yenilerini ekler.
        /// </summary>
        /// <param name="roleId">İzin atanacak rolün ID'si.</param>
        /// <param name="permissionIds">Atanacak izinlerin ID listesi.</param>
        /// <returns>İşlemin başarı durumunu (true/false) içeren bir Task.</returns>
        Task<bool> AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds);

        /// <summary>
        /// Belirtilen kullanıcıya belirli bir izni asenkron olarak atar veya kaldırır.
        /// </summary>
        /// <param name="userId">İzin atanacak/kaldırılacak kullanıcının ID'si.</param>
        /// <param name="permissionId">Atanacak/kaldırılacak iznin ID'si.</param>
        /// <param name="isGranted">İzin veriliyorsa true, kaldırılıyorsa false (varsayılan: true).</param>
        /// <returns>İşlemin başarı durumunu (true/false) içeren bir Task.</returns>
        Task<bool> AssignPermissionToUserAsync(int userId, int permissionId, bool isGranted = true);

        /// <summary>
        /// Belirtilen kullanıcının belirli bir özel iznini asenkron olarak kaldırır.
        /// </summary>
        /// <param name="userId">İzni kaldırılacak kullanıcının ID'si.</param>
        /// <param name="permissionId">Kaldırılacak iznin ID'si.</param>
        /// <returns>İşlemin başarı durumunu (true/false) içeren bir Task.</returns>
        Task<bool> RemoveUserPermissionAsync(int userId, int permissionId);

        /// <summary>
        /// Belirtilen kullanıcının tüm özel (direkt atanmış) izinlerini asenkron olarak kaldırır,
        /// böylece kullanıcı sadece rolünden gelen izinlere sahip olur.
        /// </summary>
        /// <param name="userId">İzinleri sıfırlanacak kullanıcının ID'si.</param>
        /// <returns>İşlemin başarı durumunu (true/false) içeren bir Task.</returns>
        Task<bool> ResetUserToRolePermissionsAsync(int userId);

        /// <summary>
        /// Belirtilen kullanıcının, belirtilen ada sahip bir izne (doğrudan veya rolü aracılığıyla)
        /// sahip olup olmadığını asenkron olarak kontrol eder.
        /// </summary>
        /// <param name="userId">Kontrol edilecek kullanıcının ID'si.</param>
        /// <param name="permissionName">Kontrol edilecek iznin adı (örn. "Users.View").</param>
        /// <returns>Kullanıcı izne sahipse true, değilse false içeren bir Task.</returns>
        Task<bool> UserHasPermissionAsync(int userId, string permissionName);
    }
} 