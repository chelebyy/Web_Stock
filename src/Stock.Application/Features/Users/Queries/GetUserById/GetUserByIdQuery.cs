using Stock.Application.Common;
using Stock.Application.Models;

namespace Stock.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IQuery<UserDto>
    {
        public int Id { get; set; }
    }
} 