using AutoMapper;
using MediatR;
using Stock.Application.Features.Categories.DTOs;
using Stock.Domain.Entities; // Category için
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications.Categories; // CategoryByIdSpecification için
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; // Loglama için
using Stock.Application.Common.Interfaces;

namespace Stock.Application.Features.Categories.Queries
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCategoryByIdQueryHandler> _logger;
        private readonly ICacheService _cacheService;

        public GetCategoryByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<GetCategoryByIdQueryHandler> logger,
            ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"categories:{request.Id}";

            return await _cacheService.GetOrCreateAsync(cacheKey, async () =>
            {
                _logger.LogInformation("ID: {CategoryId} olan kategori veritabanından getiriliyor.", request.Id);
                var spec = new CategoryByIdSpecification(request.Id);
                var category = await _unitOfWork.GetRepository<Category>().FirstOrDefaultAsync(spec, cancellationToken);

                if (category == null)
                {
                    _logger.LogWarning("ID: {CategoryId} olan kategori bulunamadı.", request.Id);
                    return null;
                }

                var categoryDto = _mapper.Map<CategoryDto>(category);
                _logger.LogInformation("ID: {CategoryId} olan kategori başarıyla getirildi.", request.Id);
                return categoryDto;
            }, TimeSpan.FromMinutes(30), cancellationToken: cancellationToken);
        }
    }
} 