using MediatR;
using Stock.Application.Models.DTOs;
using System.Collections.Generic;

namespace Stock.Application.Features.Users.Queries
{
    public class GetUserSummariesQuery : IRequest<IEnumerable<UserSummaryDto>>
    {
        // Sorgu parametreleri buraya eklenebilir
    }
} 