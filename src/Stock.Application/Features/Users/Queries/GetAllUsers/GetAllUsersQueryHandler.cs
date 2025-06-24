using AutoMapper;
using MediatR;
using Stock.Application.Interfaces;
using Stock.Application.Models;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications;
using Stock.Domain.Specifications.Users;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PagedResponse<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var spec = new UsersWithRolesSpecification(request.Name, request.RoleId, request.SortField, request.SortOrder);
            var countSpec = new UsersWithRolesSpecification(request.Name, request.RoleId);

            var totalRecords = await _unitOfWork.Users.CountAsync(countSpec);
            
            spec.ApplyPaging((request.PageNumber - 1) * request.PageSize, request.PageSize);
            
            var users = await _unitOfWork.Users.ListAsync(spec);

            var userDtos = _mapper.Map<IReadOnlyList<UserDto>>(users);

            return new PagedResponse<UserDto>(userDtos, request.PageNumber, request.PageSize, totalRecords);
        }
    }
} 