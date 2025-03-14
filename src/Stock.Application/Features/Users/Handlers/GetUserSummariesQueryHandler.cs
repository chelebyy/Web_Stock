using AutoMapper;
using MediatR;
using Stock.Application.Features.Users.Queries;
using Stock.Application.Models.DTOs;
using Stock.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Users.Handlers
{
    public class GetUserSummariesQueryHandler : IRequestHandler<GetUserSummariesQuery, IEnumerable<UserSummaryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserSummariesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserSummaryDto>> Handle(GetUserSummariesQuery request, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.Users.GetUserSummariesAsync();
            return _mapper.Map<IEnumerable<UserSummaryDto>>(users);
        }
    }
} 