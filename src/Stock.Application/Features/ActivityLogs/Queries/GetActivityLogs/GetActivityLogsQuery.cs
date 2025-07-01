using MediatR;
using Stock.Application.Models;
using Stock.Application.Models.DTOs;

namespace Stock.Application.Features.ActivityLogs.Queries.GetActivityLogs
{
    public class GetActivityLogsQuery : IRequest<PagedResponse<ActivityLogDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
} 