using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Models;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

        public GetAllRolesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllRolesQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Handles the query to retrieve all roles.
        /// </summary>
        /// <param name="request">The query request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A collection of <see cref="RoleSlimDto"/>.</returns>
        public async Task<PagedResponse<RoleSlimDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Roller getiriliyor: Sayfa={PageNumber}, Boyut={PageSize}, Sıralama={SortField} {SortOrder}, Filtre={Name}",
                request.PageNumber, request.PageSize, request.SortField, request.SortOrder, request.Name);

            var spec = new RolesSpecification(request.Name, request.SortField, request.SortOrder);
            var countSpec = new RolesSpecification(request.Name, null, null); // Sayım için sıralama gerekmez

            spec.ApplyPaging((request.PageNumber - 1) * request.PageSize, request.PageSize);

            var totalRecords = await _unitOfWork.Roles.CountAsync(countSpec);
            var roles = await _unitOfWork.Roles.ListAsync(spec);

            var roleDtos = _mapper.Map<IEnumerable<RoleSlimDto>>(roles);
            
            _logger.LogInformation("{Count} adet rol getirildi.", roleDtos.Count());

            return new PagedResponse<RoleSlimDto>(roleDtos, request.PageNumber, request.PageSize, totalRecords);
        }
    }
} 