using MediatR;
using Stock.Application.Models.DTOs;

namespace Stock.Application.Features.Users.Commands
{
    /// <summary>
    /// Kullanıcı rolünü güncellemek için komut
    /// </summary>
    public class UpdateUserRoleCommand : IRequest<UserDto>
    {
        /// <summary>
        /// Kullanıcı ID
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Yeni rol ID
        /// </summary>
        public int RoleId { get; set; }
    }
} 