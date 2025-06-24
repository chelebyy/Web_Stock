using System;
using System.Collections.Generic;

namespace Stock.Application.Models.DTOs
{
    /// <summary>
    /// Rol verilerini temsil eden Data Transfer Object (DTO).
    /// </summary>
    public class RoleDto
    {
        /// <summary>
        /// Rol ID'si.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Rol adı.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Rol açıklaması.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Bu role sahip kullanıcı sayısı.
        /// </summary>
        public int UserCount { get; set; }

        public List<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();

        // Detay görünümlerinde gerekirse kullanıcı listesi de eklenebilir
        // public List<UserSummaryDto> Users { get; set; } = new List<UserSummaryDto>();
    }
} 