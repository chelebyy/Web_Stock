using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Models;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Stock.Application.Common.Interfaces;
using System;

namespace Stock.Application.Features.Roles.Queries.GetAllRoles
{
    /// <summary>
    /// Handles the <see cref="GetAllRolesQuery"/>.
    /// </summary>
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, PagedResponse<RoleSlimDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllRolesQueryHandler> _logger;
        private readonly ICacheService _cacheService;

        public GetAllRolesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllRolesQueryHandler> logger, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Handles the query to retrieve all roles.
        /// </summary>
        /// <param name="request">The query request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A collection of <see cref="RoleSlimDto"/>.</returns>
        public async Task<PagedResponse<RoleSlimDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"roles_page_{request.PageNumber}_size_{request.PageSize}_sort_{request.SortField}_{request.SortOrder}_name_{request.Name}";

            return await _cacheService.GetOrCreateAsync(cacheKey, async () =>
            {
                _logger.LogInformation("Cache miss for roles with key: {CacheKey}. Fetching from database.", cacheKey);

                var countSpec = new RolesSpecification(request.Name);
                var pagedSpec = new RolesSpecification(
                    request.Name,
                    request.SortField,
                    request.SortOrder,
                    request.PageNumber,
                    request.PageSize);
                
                _logger.LogInformation("Fetching total count of roles from database.");
                var totalRecords = await _unitOfWork.Roles.CountAsync(countSpec, cancellationToken);
                
                _logger.LogInformation("Fetching paged list of roles from database.");
                var roles = await _unitOfWork.Roles.ListAsync(pagedSpec, cancellationToken);

                var rolesDto = _mapper.Map<IEnumerable<RoleSlimDto>>(roles);

                _logger.LogInformation("{Count} roles returned after database query.", rolesDto.Count());

                return new PagedResponse<RoleSlimDto>(rolesDto, request.PageNumber, request.PageSize, totalRecords);

            }, slidingExpiration: TimeSpan.FromMinutes(10));
        }
    }
} 