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
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteCategoryCommandHandler> _logger;
        private readonly ICacheService _cacheService;

        public DeleteCategoryCommandHandler(
            ICategoryRepository categoryRepository, 
            IUnitOfWork unitOfWork, 
            ILogger<DeleteCategoryCommandHandler> logger,
            ICacheService cacheService)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ID: {CategoryId} olan kategori siliniyor.", request.Id);

            var spec = new CategoryByIdSpecification(request.Id);
            var category = await _categoryRepository.FirstOrDefaultAsync(spec, cancellationToken);

            if (category == null)
            {
                _logger.LogWarning("Silinecek kategori bulunamadı: ID {CategoryId}", request.Id);
                return Result.Failure($"Category with ID {request.Id} not found.");
            }

            try
            {
                await _categoryRepository.DeleteAsync(category, cancellationToken);
                var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

                if (saveResult <= 0)
                {
                    _logger.LogError("Kategori silinirken veritabanına kaydetme başarısız: ID {CategoryId}", request.Id);
                    return Result.Failure("Failed to delete the category from the database.");
                }

                // Invalidate both the list cache and the specific item cache
                var listCachePrefix = "categories_page";
                await _cacheService.RemoveByPrefixAsync(listCachePrefix).ConfigureAwait(false);
                _logger.LogInformation("Cache invalidated for prefix: {CachePrefix}", listCachePrefix);

                var specificCacheKey = $"categories:{request.Id}";
                await _cacheService.RemoveAsync(specificCacheKey).ConfigureAwait(false);
                _logger.LogInformation("Cache invalidated for key: '{SpecificCacheKey}'", specificCacheKey);

                _logger.LogInformation("ID: {CategoryId} olan kategori başarıyla silindi.", request.Id);
                return Result.Success();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Kategori silinirken bir hata oluştu: ID {CategoryId}", request.Id);
                return Result.Failure("Failed to delete the category from the database.");
            }
        }
    }
} 