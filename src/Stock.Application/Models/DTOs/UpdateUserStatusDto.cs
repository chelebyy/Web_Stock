using System;

namespace Stock.Application.Models.DTOs
{
    /// <summary>
    /// Kullanıcı durumu güncelleme işlemi için DTO
    /// </summary>
    public class UpdateUserStatusDto
    {
        /// <summary>
        /// Kullanıcının aktif olup olmadığı
        /// </summary>
        public bool IsActive { get; set; }
    }
} 