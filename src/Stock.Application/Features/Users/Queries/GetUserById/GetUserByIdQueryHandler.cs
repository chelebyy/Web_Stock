using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Interfaces; // IUnitOfWork
using Stock.Application.Models.DTOs; // UserDto
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications.Users; // UserWithRoleAndPermissionsSpecification için
using System.Threading;
using System.Threading.Tasks;
using AutoMapper; // AutoMapper için
using Stock.Application.Common.Interfaces;

namespace Stock.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly ILogger<GetUserByIdQueryHandler> _logger;

        public GetUserByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService, ILogger<GetUserByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"users:{request.Id}";

            return await _cacheService.GetOrCreateAsync(cacheKey, async () =>
            {
                _logger.LogInformation("Fetching user with ID: {UserId} from database.", request.Id);
                var spec = new UserWithRoleAndPermissionsSpecification(request.Id);

                var user = await _unitOfWork.GetRepository<User>().FirstOrDefaultAsync(spec, cancellationToken);

                if (user == null)
                {
                    _logger.LogWarning("User with ID: {UserId} not found in database.", request.Id);
                    return null;
                }

                _logger.LogInformation("User with ID: {UserId} fetched successfully from database.", request.Id);
                return _mapper.Map<UserDto>(user);

            }, TimeSpan.FromMinutes(30), cancellationToken: cancellationToken);
        }
    }
} 