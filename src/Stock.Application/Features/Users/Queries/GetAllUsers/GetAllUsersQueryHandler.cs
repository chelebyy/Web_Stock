using AutoMapper;
using MediatR;
using Stock.Application.Interfaces;
using Stock.Application.Models;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Interfaces;
using System;

namespace Stock.Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PagedResponse<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllUsersQueryHandler> _logger;
        private readonly ICacheService _cacheService;

        public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllUsersQueryHandler> logger, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<PagedResponse<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"users_page_{request.PageNumber}_size_{request.PageSize}_sort_{request.SortField}_{request.SortOrder}_name_{request.Name}_role_{request.RoleId}";
            
            _logger.LogInformation("Attempting to fetch users from cache with key: {CacheKey}", cacheKey);

            return await _cacheService.GetOrCreateAsync(cacheKey, async () =>
            {
                _logger.LogInformation("Cache miss for key: {CacheKey}. Fetching users from database.", cacheKey);

                var countSpec = new UsersWithRolesSpecification(request.Name, request.RoleId, null, null);
                var pagedSpec = new UsersWithRolesSpecification(
                    request.Name, 
                    request.RoleId, 
                    request.SortField, 
                    request.SortOrder, 
                    request.PageNumber, 
                    request.PageSize);

                var totalRecords = await _unitOfWork.Users.CountAsync(countSpec, cancellationToken);
                var users = await _unitOfWork.Users.ListAsync(pagedSpec, cancellationToken);
                
                var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);

                _logger.LogInformation("{Count} users fetched from database and cached.", userDtos.Count());
                
                return new PagedResponse<UserDto>(userDtos, request.PageNumber, request.PageSize, totalRecords);

            }, slidingExpiration: TimeSpan.FromMinutes(5));
        }
    }
} 