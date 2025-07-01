using System;
using Stock.Domain.Common;
using Stock.Domain.Entities;

namespace Stock.Domain.Entities.Permissions
{
    public class UserPermission : BaseEntity
    {
        public int UserId { get; private set; }
        public int PermissionId { get; private set; }
        public bool IsGranted { get; private set; } = true;

        // Navigation properties
        public User User { get; private set; }
        public Permission Permission { get; private set; }

        // EF Core için protected constructor
        protected UserPermission() 
        {
            User = null!;
            Permission = null!;
        }

        // Primary constructor
        private UserPermission(int userId, int permissionId, bool isGranted)
        {
            UserId = userId;
            PermissionId = permissionId;
            IsGranted = isGranted;
        }

        // Factory metodu
        public static UserPermission Create(int userId, int permissionId, bool isGranted = true)
        {
            return new UserPermission(userId, permissionId, isGranted);
        }

        // Davranış metotları
        public void UpdateGrantStatus(bool isGranted)
        {
            IsGranted = isGranted;
        }
    }
} 