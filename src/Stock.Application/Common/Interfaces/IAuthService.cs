using Stock.Application.Models.DTOs;
using System.Threading.Tasks;

namespace Stock.Application.Common.Interfaces
{
    /// <summary>
    /// Kullanıcı kimlik doğrulama ve yetkilendirme işlemleri için arayüz.
    /// Login, register, token oluşturma ve şifre değiştirme gibi temel kimlik doğrulama operasyonlarını tanımlar.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Kullanıcı girişi işlemini asenkron olarak gerçekleştirir.
        /// </summary>
        /// <param name="loginDto">Kullanıcının giriş bilgilerini (kullanıcı adı/sicil ve şifre) içeren DTO.</param>
        /// <returns>
        /// Giriş işleminin sonucunu içeren bir <see cref="AuthResponseDto"/> nesnesi.
        /// Başarılı girişte token ve kullanıcı bilgilerini içerir.
        /// </returns>
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);

        /// <summary>
        /// Yeni kullanıcı kaydı işlemini asenkron olarak gerçekleştirir.
        /// </summary>
        /// <param name="registerDto">Yeni kullanıcının kayıt bilgilerini içeren DTO.</param>
        /// <returns>
        /// Kayıt işleminin sonucunu içeren bir <see cref="AuthResponseDto"/> nesnesi.
        /// Başarılı kayıtta genellikle token ve yeni kullanıcı bilgilerini içerir.
        /// </returns>
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);

        /// <summary>
        /// Belirtilen kullanıcı bilgileri için bir JWT (JSON Web Token) oluşturur.
        /// </summary>
        /// <param name="user">Token oluşturulacak kullanıcı bilgilerini içeren <see cref="UserDto"/>.</param>
        /// <returns>Oluşturulan JWT token string'i.</returns>
        string GenerateJwtToken(UserDto user);

        /// <summary>
        /// Belirtilen kullanıcının şifresini asenkron olarak değiştirir.
        /// </summary>
        /// <param name="changePasswordDto">Eski ve yeni şifre bilgilerini içeren DTO.</param>
        /// <param name="userId">Şifresi değiştirilecek kullanıcının ID'si.</param>
        /// <returns>
        /// Şifre değiştirme işleminin sonucunu içeren bir <see cref="AuthResponseDto"/> nesnesi.
        /// </returns>
        Task<AuthResponseDto> ChangePasswordAsync(ChangePasswordDto changePasswordDto, int userId);
    }
} 