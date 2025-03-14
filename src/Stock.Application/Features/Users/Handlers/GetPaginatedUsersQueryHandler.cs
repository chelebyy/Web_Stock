using AutoMapper;
using MediatR;
using Stock.Application.Features.Users.Queries.GetPaginatedUsers;
using Stock.Application.Models.DTOs;
using Stock.Domain.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Users.Handlers
{
    public class GetPaginatedUsersQueryHandler : IRequestHandler<GetPaginatedUsersQuery, PaginatedResult<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPaginatedUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<UserDto>> Handle(GetPaginatedUsersQuery request, CancellationToken cancellationToken)
        {
            var (users, totalCount) = await _unitOfWork.Users.GetPaginatedUsersAsync(request.PageNumber, request.PageSize);
            
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            
            return new PaginatedResult<UserDto>
            {
                Items = userDtos,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
} 