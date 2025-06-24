namespace Stock.Application.Models.DTOs
{
    /// <summary>
    /// Kullanıcı listelerinde gösterilecek temel kullanıcı bilgilerini içeren DTO.
    /// UserDto'ya göre daha hafiftir ve sadece liste için gerekli verileri taşır.
    /// </summary>
    public class UserListItemDto
    {
        public int Id { get; set; }
        public string Adi { get; set; } = string.Empty;
        public string Soyadi { get; set; } = string.Empty;
        public string Sicil { get; set; } = string.Empty;
        public string? RoleName { get; set; }
    }
} 