using MediatR;
using Stock.Domain.Common; // Result için
using Stock.Domain.Entities; // Category için
using Stock.Domain.Interfaces; // IUnitOfWork ve ICategoryRepository için
using Stock.Domain.Specifications.Categories; // CategoryByIdSpecification için
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; // Loglama için
using Stock.Domain.Exceptions; // DomainErrors için
using Stock.Application.Common.Interfaces; // ICacheService için

namespace Stock.Application.Features.Categories.Commands
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateCategoryCommandHandler> _logger;
        private readonly ICacheService _cacheService;

        public UpdateCategoryCommandHandler(
            ICategoryRepository categoryRepository, 
            IUnitOfWork unitOfWork, 
            ILogger<UpdateCategoryCommandHandler> logger,
            ICacheService cacheService)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ID: {CategoryId} olan kategori güncelleniyor.", request.Id);

            var spec = new CategoryByIdSpecification(request.Id);
            var category = await _categoryRepository.FirstOrDefaultAsync(spec, cancellationToken);

            if (category == null)
            {
                _logger.LogWarning("Güncellenecek kategori bulunamadı: ID {CategoryId}", request.Id);
                return Result.Failure($"Category with ID {request.Id} not found.");
            }

            var nameUpdateResult = category.UpdateName(request.Name);
            if (!nameUpdateResult.IsSuccess)
            {
                _logger.LogWarning("Kategori adı güncelleme başarısız (Domain Validation): {Error}", nameUpdateResult.Error);
                return Result.Failure(nameUpdateResult.Error);
            }

            var descriptionUpdateResult = category.UpdateDescription(request.Description);
            if (!descriptionUpdateResult.IsSuccess)
            {
                 _logger.LogWarning("Kategori açıklaması güncelleme başarısız (Domain Validation): {Error}", descriptionUpdateResult.Error);
                return Result.Failure(descriptionUpdateResult.Error);
            }

            try
            {
                var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

                if (saveResult <= 0)
                {
                    _logger.LogError("Kategori güncellenirken veritabanına kaydetme başarısız: ID {CategoryId}", request.Id);
                    return Result.Failure("Failed to update the category in the database.");
                }

                // Invalidate both the list cache and the specific item cache
                var listCachePrefix = "categories_page";
                await _cacheService.RemoveByPrefixAsync(listCachePrefix).ConfigureAwait(false);
                _logger.LogInformation("Cache invalidated for prefix: {CachePrefix}", listCachePrefix);

                var specificCacheKey = $"categories:{request.Id}";
                await _cacheService.RemoveAsync(specificCacheKey).ConfigureAwait(false);
                _logger.LogInformation("Cache invalidated for key: '{SpecificCacheKey}'", specificCacheKey);

                _logger.LogInformation("ID: {CategoryId} olan kategori başarıyla güncellendi.", request.Id);
                return Result.Success();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Kategori güncellenirken bir hata oluştu: ID {CategoryId}", request.Id);
                return Result.Failure("Failed to update the category in the database.");
            }
        }
    }
} 