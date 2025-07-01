using System;
using System.Collections.Generic;
using Stock.Domain.Common;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.Exceptions;
using Stock.Domain.ValueObjects;

namespace Stock.Domain.Entities
{
    public class User : BaseEntity
    {
        // Entity'nin iç durumu private setter'lar ile korunuyor
        public FullName FullName { get; private set; }
        public Sicil Sicil { get; private set; }
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsAdmin { get; private set; }
        public DateTime? LastLoginAt { get; private set; }

        // Foreign key to Role
        public int? RoleId { get; private set; }
        public Role? Role { get; private set; }

        // Navigation property
        public List<UserPermission> UserPermissions { get; private set; } = new List<UserPermission>();

        // Boş constructor Entity Framework için gerekli
        protected User()
        {
            FullName = null!;
            Sicil = null!;
            Username = null!;
            PasswordHash = null!;
            UserPermissions = new List<UserPermission>();
        }

        // Private constructor ile nesne oluşturma kontrolü
        private User(FullName fullName, Sicil sicil, string passwordHash, int? roleId, bool isAdmin)
        {
            FullName = fullName;
            Sicil = sicil;
            Username = sicil.Value;
            PasswordHash = passwordHash;
            RoleId = roleId;
            IsAdmin = isAdmin;
            IsActive = true;
            UserPermissions = new List<UserPermission>();
        }

        // Factory metodu - Entity oluşturmayı kontrol ediyor
        public static Result<User> Create(
            FullName fullName, 
            Sicil sicil, 
            string passwordHash, 
            int? roleId = null,
            bool isAdmin = false)
        {
            if (fullName == null) return Result<User>.Failure("Full name cannot be null.");
            if (sicil == null) return Result<User>.Failure("Sicil cannot be null.");
            if (string.IsNullOrWhiteSpace(passwordHash)) 
                return Result<User>.Failure("Password hash cannot be empty.");

            return Result<User>.Success(new User(fullName, sicil, passwordHash, roleId, isAdmin));
        }

        // Domain davranışları - Entity'nin iş kurallarını zorunlu kılıyor
        public Result UpdateFullName(FullName newFullName)
        {
            if (newFullName == null) return Result.Failure("Full name cannot be null.");
            FullName = newFullName;
            return Result.Success();
        }

        public Result UpdatePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                return Result.Failure("Password hash cannot be empty.");
            
            PasswordHash = newPasswordHash;
            return Result.Success();
        }

        public Result ChangePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                return Result.Failure("Password hash cannot be empty.");
            
            PasswordHash = newPasswordHash;
            return Result.Success();
        }

        public Result UpdateSicil(Sicil newSicil)
        {
            if (newSicil == null) return Result.Failure("Sicil cannot be null.");
            Sicil = newSicil;
            Username = newSicil.Value; // Username'i de güncelle
            return Result.Success();
        }

        public Result UpdateName(FullName newFullName)
        {
            if (newFullName == null) return Result.Failure("Full name cannot be null.");
            FullName = newFullName;
            return Result.Success();
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void MakeAdmin()
        {
            IsAdmin = true;
        }

        public void RemoveAdmin()
        {
            IsAdmin = false;
        }

        public void SetAdminStatus(bool isAdmin)
        {
            IsAdmin = isAdmin;
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

        public void RecordLoginTime(DateTime loginTime)
        {
            LastLoginAt = loginTime;
        }

        // UserPermissions koleksiyonunu yönetmek için metotlar
        public void AddPermission(UserPermission permission)
        {
            if (!UserPermissions.Any(up => up.PermissionId == permission.PermissionId))
            {
                UserPermissions.Add(permission);
            }
        }

        public void RemovePermission(int permissionId)
        {
            var permission = UserPermissions.FirstOrDefault(up => up.PermissionId == permissionId);
            if (permission != null)
            {
                UserPermissions.Remove(permission);
            }
        }

        public bool HasPermission(int permissionId)
        {
            // Check direct permissions
            if (UserPermissions.Any(up => up.PermissionId == permissionId))
                return true;

            // Check role-based permissions
            if (Role?.RolePermissions.Any(rp => rp.PermissionId == permissionId) == true)
                return true;

            return false;
        }

        // İlave kullanışlı metot
        public string GetFullName()
        {
            return FullName.ToString();
        }
    }
} 