using System;

namespace Stock.Application.Models.DTOs
{
    /// <summary>
    /// Kullanıcı rol değiştirme işlemi için DTO
    /// </summary>
    public class ChangeRoleDto
    {
        /// <summary>
        /// Yeni rol ID'si
        /// </summary>
        public int RoleId { get; set; }
    }
} 