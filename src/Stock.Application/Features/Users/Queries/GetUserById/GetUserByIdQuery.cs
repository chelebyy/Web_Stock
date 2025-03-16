using Stock.Application.Common.CQRS;
using Stock.Application.Features.Users.Dtos;

namespace Stock.Application.Features.Users.Queries.GetUserById
{
    /// <summary>
    /// ID'ye göre kullanıcı sorgulama
    /// </summary>
    public class GetUserByIdQuery : IQuery<UserDto>
    {
        /// <summary>
        /// Kullanıcı ID
        /// </summary>
        public int Id { get; set; }
    }
} 