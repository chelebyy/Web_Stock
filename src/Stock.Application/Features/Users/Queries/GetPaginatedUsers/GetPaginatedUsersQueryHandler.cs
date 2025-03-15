using AutoMapper;
using MediatR;
using Stock.Application.Interfaces;
using Stock.Application.Models.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Stock.Application.Features.Users.Queries.GetPaginatedUsers
{
    public class GetPaginatedUsersQueryHandler : IRequestHandler<GetPaginatedUsersQuery, PaginatedResult<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPaginatedUsersQueryHandler> _logger;

        public GetPaginatedUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPaginatedUsersQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PaginatedResult<UserDto>> Handle(GetPaginatedUsersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Sayfalanmış kullanıcılar getiriliyor. Sayfa: {request.PageNumber}, Boyut: {request.PageSize}");
            
            // Performans ölçümü için Stopwatch başlat
            var stopwatch = Stopwatch.StartNew();
            
            // Filtreleme parametrelerini UserRepository'ye ilet
            var (users, totalCount) = await _unitOfWork.Users.GetPaginatedUsersAsync(
                request.PageNumber, 
                request.PageSize,
                request.UsernameFilter,
                request.SicilFilter,
                request.RoleIdFilter,
                request.IsActiveFilter,
                request.IsAdminFilter,
                request.SortBy,
                request.SortAscending);
            
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            
            // Stopwatch'ı durdur ve geçen süreyi hesapla
            stopwatch.Stop();
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            
            _logger.LogInformation($"Toplam {totalCount} kullanıcıdan {userDtos.Count()} kullanıcı {elapsedMs}ms sürede getirildi.");
            
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