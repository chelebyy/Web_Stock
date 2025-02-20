using System;

namespace Stock.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
        public int? RoleId { get; set; }
        public virtual Role Role { get; set; }

        public User()
        {
            IsAdmin = false;
        }
    }
} 