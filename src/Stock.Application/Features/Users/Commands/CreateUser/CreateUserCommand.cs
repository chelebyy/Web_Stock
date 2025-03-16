using Stock.Application.Common.CQRS;

namespace Stock.Application.Features.Users.Commands.CreateUser
{
    /// <summary>
    /// Yeni kullanıcı oluşturma komutu
    /// </summary>
    public class CreateUserCommand : ICommand<int>
    {
        /// <summary>
        /// Kullanıcı adı
        /// </summary>
        public string Username { get; set; }
        
        /// <summary>
        /// Şifre
        /// </summary>
        public string Password { get; set; }
        
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
    }
} 