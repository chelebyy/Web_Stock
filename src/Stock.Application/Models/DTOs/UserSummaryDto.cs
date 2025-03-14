namespace Stock.Application.Models.DTOs
{
    /// <summary>
    /// Kullanıcı özet bilgilerini içeren DTO sınıfı.
    /// Projection için kullanılır ve sadece gerekli alanları içerir.
    /// </summary>
    public class UserSummaryDto
    {
        /// <summary>
        /// Kullanıcı ID'si
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Kullanıcı adı
        /// </summary>
        public string Username { get; set; } = string.Empty;
        
        /// <summary>
        /// Kullanıcının rol adı
        /// </summary>
        public string RoleName { get; set; } = string.Empty;
        
        /// <summary>
        /// Kullanıcının e-posta adresi
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Kullanıcının sicil numarası
        /// </summary>
        public string Sicil { get; set; } = string.Empty;
        
        /// <summary>
        /// Kullanıcının aktif olup olmadığı
        /// </summary>
        public bool IsActive { get; set; }
    }
} 