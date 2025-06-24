using System;
using System.Collections.Generic;
using Stock.Domain.Common;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.Exceptions;

namespace Stock.Domain.Entities
{
    public class User : BaseEntity
    {
        // Entity'nin iç durumu private setter'lar ile korunuyor
        public string Adi { get; private set; } = string.Empty;
        public string Soyadi { get; private set; } = string.Empty;
        public string Sicil { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public bool IsAdmin { get; private set; }
        public DateTime? LastLoginAt { get; private set; }

        public int? RoleId { get; private set; }
        public Role? Role { get; private set; }

        public ICollection<UserPermission> UserPermissions { get; private set; } = new List<UserPermission>();

        // Boş constructor Entity Framework için gerekli
        protected User()
        {
            UserPermissions = new List<UserPermission>();
        }

        // Private constructor ile nesne oluşturma kontrolü
        private User(string adi, string soyadi, string sicil, string passwordHash, int? roleId, bool isAdmin)
        {
            Adi = adi;
            Soyadi = soyadi;
            Sicil = sicil;
            PasswordHash = passwordHash;
            RoleId = roleId;
            IsAdmin = isAdmin;
            UserPermissions = new List<UserPermission>();
        }

        // Factory metodu - Entity oluşturmayı kontrol ediyor
        public static Result<User> Create(
            string adi, 
            string soyadi, 
            string sicil, 
            string passwordHash,
            int? roleId = null, 
            bool isAdmin = false)
        {
            if (string.IsNullOrWhiteSpace(adi))
            {
                return Result<User>.Failure(DomainErrors.User.AdiEmpty);
            }

            if (string.IsNullOrWhiteSpace(soyadi))
            {
                return Result<User>.Failure(DomainErrors.User.SoyadiEmpty);
            }

            if (string.IsNullOrWhiteSpace(sicil))
            {
                return Result<User>.Failure(DomainErrors.User.SicilEmpty);
            }

            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                return Result<User>.Failure(DomainErrors.User.PasswordHashEmpty);
            }

            var user = new User(adi, soyadi, sicil, passwordHash, roleId, isAdmin);
            return Result<User>.Success(user);
        }

        // Domain davranışları - Entity'nin iş kurallarını zorunlu kılıyor
        public Result UpdateName(string newAdi, string newSoyadi)
        {
            if (string.IsNullOrWhiteSpace(newAdi))
            {
                return Result.Failure(DomainErrors.User.AdiEmpty);
            }

            if (string.IsNullOrWhiteSpace(newSoyadi))
            {
                return Result.Failure(DomainErrors.User.SoyadiEmpty);
            }

            Adi = newAdi;
            Soyadi = newSoyadi;
            return Result.Success();
        }

        public Result UpdateSicil(string newSicil)
        {
            if (string.IsNullOrWhiteSpace(newSicil))
            {
                return Result.Failure(DomainErrors.User.SicilEmpty);
            }
            Sicil = newSicil;
            return Result.Success();
        }

        public Result ChangePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
            {
                return Result.Failure(DomainErrors.User.PasswordHashEmpty);
            }
            PasswordHash = newPasswordHash;
            return Result.Success();
        }

        public void AssignRole(int roleId, Role? role)
        {
            RoleId = roleId;
            Role = role;
        }

        public void RemoveRole()
        {
            RoleId = null;
            Role = null;
        }

        public void SetAdminStatus(bool isAdmin)
        {
            IsAdmin = isAdmin;
        }

        public void RecordLoginTime(DateTime loginTime)
        {
            LastLoginAt = loginTime;
        }

        // UserPermissions koleksiyonunu yönetmek için metotlar
        public void AddPermission(UserPermission permission)
        {
            // Belki burada duplicate kontrolü eklenebilir?
            UserPermissions.Add(permission);
        }

        public void RemovePermission(UserPermission permission)
        {
            UserPermissions.Remove(permission);
        }

        public void ClearPermissions()
        {
            UserPermissions.Clear();
        }

        // İlave kullanışlı metot
        public string GetFullName()
        {
            return $"{Adi} {Soyadi}";
        }
    }
} 