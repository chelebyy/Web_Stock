using System;

namespace Stock.Application.Features.Users.Dtos
{
    /// <summary>
    /// Kullanıcı veri transfer nesnesi
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Kullanıcı ID
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Kullanıcı adı
        /// </summary>
        public string Username { get; set; }
        
        /// <summary>
        /// E-posta adresi
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Tam ad
        /// </summary>
        public string FullName { get; set; }
        
        /// <summary>
        /// Rol ID
        /// </summary>
        public int RoleId { get; set; }
        
        /// <summary>
        /// Rol adı
        /// </summary>
        public string RoleName { get; set; }
        
        /// <summary>
        /// Admin mi?
        /// </summary>
        public bool IsAdmin { get; set; }
        
        /// <summary>
        /// Oluşturulma tarihi
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Güncellenme tarihi
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
} 