using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Interfaces;
using Stock.Domain.Common;
using Stock.Domain.Interfaces;
using Stock.Domain.Entities;
using Stock.Domain.ValueObjects;
using System.Threading;
using System.Threading.Tasks;
using Stock.Domain.Specifications.Products;
using Stock.Domain.Specifications.Categories;

namespace Stock.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateProductCommandHandler> _logger;
        private readonly ICacheService _cacheService;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateProductCommandHandler> logger, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var productRepo = _unitOfWork.GetRepository<Product>();
            var product = await productRepo.FirstOrDefaultAsync(new ProductByIdSpecification(request.Id), cancellationToken).ConfigureAwait(false);

            if (product == null)
            {
                _logger.LogWarning("Product to update not found with ID: {ProductId}", request.Id);
                return Result.Failure(DomainErrors.Product.NotFound(request.Id));
            }

            // Kategori değişikliği kontrolü
            if (request.CategoryId != product.CategoryId)
            {
                var categorySpec = new CategoryByIdSpecification(request.CategoryId);
                var category = await _unitOfWork.Categories.FirstOrDefaultAsync(categorySpec, cancellationToken).ConfigureAwait(false);
                if (category == null)
                {
                    return Result.Failure(DomainErrors.Category.NotFound(request.CategoryId));
                }
                var changeCategoryResult = product.ChangeCategory(request.CategoryId);
                if(!changeCategoryResult.IsSuccess) return changeCategoryResult;
            }

            // Diğer alanların güncellenmesi
            var nameValueObject = ProductName.From(request.Name);
            var descriptionValueObject = ProductDescription.From(request.Description);

            var updateNameResult = product.UpdateName(nameValueObject);
            if (!updateNameResult.IsSuccess) return updateNameResult;

            var updateDescriptionResult = product.UpdateDescription(descriptionValueObject);
            if (!updateDescriptionResult.IsSuccess) return updateDescriptionResult;
            
            // Stok seviyesi güncellemesi
            if (request.StockLevel != product.StockLevel.Value)
            {
                int difference = request.StockLevel - product.StockLevel.Value;
                Result stockUpdateResult;
                if (difference > 0)
                {
                    stockUpdateResult = product.IncreaseStock(difference);
                }
                else
                {
                    stockUpdateResult = product.DecreaseStock(-difference);
                }
                if (!stockUpdateResult.IsSuccess) return stockUpdateResult;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // Invalidate both the list cache and the specific item cache
            var listCachePrefix = "products_page";
            await _cacheService.RemoveByPrefixAsync(listCachePrefix).ConfigureAwait(false);
            _logger.LogInformation("Cache invalidated for prefix: {CachePrefix}", listCachePrefix);

            var cacheKey = $"products:{product.Id}";
            await _cacheService.RemoveAsync(cacheKey).ConfigureAwait(false);
            _logger.LogInformation("Cache invalidated for key: {CacheKey}", cacheKey);
            
            _logger.LogInformation("Product {ProductId} updated successfully.", product.Id);

            return Result.Success();
        }
    }
} 