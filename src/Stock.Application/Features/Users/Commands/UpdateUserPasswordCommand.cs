using MediatR;
using Stock.Application.Models.DTOs;

namespace Stock.Application.Features.Users.Commands
{
    /// <summary>
    /// Kullanıcı şifresini güncellemek için komut
    /// </summary>
    public class UpdateUserPasswordCommand : IRequest<UserDto>
    {
        /// <summary>
        /// Kullanıcı ID
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Yeni şifre
        /// </summary>
        public string Password { get; set; }
    }
} 