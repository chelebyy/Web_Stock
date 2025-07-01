using AutoMapper;
using MediatR;
using Stock.Application.Models;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.ActivityLogs.Queries.GetActivityLogs
{
    public class GetActivityLogsQueryHandler : IRequestHandler<GetActivityLogsQuery, PagedResponse<ActivityLogDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetActivityLogsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<ActivityLogDto>> Handle(GetActivityLogsQuery request, CancellationToken cancellationToken)
        {
            var spec = new ActivityLogsPaginatedSpecification(request.Page, request.PageSize);
            var logs = await _unitOfWork.ActivityLogs.ListAsync(spec, cancellationToken);
            
            var totalItemsSpec = new GetAllSpecification<ActivityLog>(); // Use the concrete spec for counting
            var totalItems = await _unitOfWork.ActivityLogs.CountAsync(totalItemsSpec, cancellationToken);

            var logsDto = _mapper.Map<IReadOnlyList<ActivityLogDto>>(logs);

            return new PagedResponse<ActivityLogDto>(logsDto, request.Page, request.PageSize, totalItems);
        }
    }
} 